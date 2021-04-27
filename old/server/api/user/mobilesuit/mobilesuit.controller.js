'use strict';

var _ = require('lodash');
var async = require('async');
var UserMobilesuit = require('./mobilesuit.model');
var User = require('../user.model');
var Serials = require('../../serials/serials.model');
var Master = require('../../../config/master');
var logger = require('../../../logger');

var validationError = function(res, statusCode, err) {
  logger.system.info('mobilesuit_ctrl:: '+ err);
  return res.status(statusCode).send(err);
};

// Get list of user/mobilesuits
exports.index = function(req, res) {
  var userId = req.user.id;
  UserMobilesuit.find({ userId: userId }, function (err, mobilesuits) {
    if(err) { return handleError(res, err); }
    return res.json(200, mobilesuits);
  });
};

// Get a single user/mobilesuit
exports.show = function(req, res) {
  UserMobilesuit.findById(req.params.id, function (err, mobilesuit) {
    if(err) { return handleError(res, err); }
    if(!mobilesuit) { return res.send(404); }
    return res.json(mobilesuit);
  });
};

// Creates a new user/mobilesuit in the DB.
exports.create = function(req, res) {
  var userId = req.user.id;
  var msId = req.body.id;
  Serials.getNewSerial('serial_mobilesuit', function (err, serial) {
    if (err || serial <= 0) {
      logger.system.info("WARN: mobilesuit_controller: cant create mobilesuit, invalid serial");
      return res.send(404);
    }

    logger.system.info("mobilesuit_controller: msid=" + msId + " serial=" + serial);
    Master.mobilesuits.findById(msId, function (err, ms) {
      if (err) { return handleError(res, err); }
      if (!ms) { return res.send(404); }

      UserMobilesuit.create({
        _id: serial,
        userId: userId,
        msId: msId,
        DisplayName: ms.DisplayName,
        inventory: [],
        exp: 0
      }, function(err, mobilesuit) {
        if(err) { return handleError(res, err); }
        return res.json(201, mobilesuit);
      });
    })
  });
};

// Add mobilesuits exp
exports.addExp = function(req, res) {
  var exp = parseInt(req.body.addExp, 10);

  UserMobilesuit.find({ $and: [{userId: req.user._id},{msId: req.body.msId}] }, function (err, mobilesuits) {
    if (err) { return handleError(res, err); }
    if (!mobilesuits || !mobilesuits.length ) {
      return validationError(res, 404, 'cant add exp, not found mobilesuit');
    }
    async.each(mobilesuits, function (ms, next) {
      var _exp = ms.exp;
      ms.exp = parseInt(ms.exp, 10) + exp;
      ms.save(function (err) {
        if (err) next(err);
        logger.system.info('mobilesuit_controller exp update [' + req.user.name + ',' + ms._id + ',' + ms.msId + '] ' + _exp + '+' + exp + '->' + ms.exp);
      });
    }, function (err) {
      if (err) return validationError(res, 422, err);
      res.send(200);
    });
  });
};

// Updates an existing user/mobilesuit in the DB.
exports.update = function(req, res) {
  if(req.body._id) { delete req.body._id; }
  UserMobilesuit.findById(req.params.id, function (err, mobilesuit) {
    if (err) { return handleError(res, err); }
    if(!mobilesuit) { return res.send(404); }
    var updated = _.merge(mobilesuit, req.body);
    updated.save(function (err) {
      if (err) { return handleError(res, err); }
      return res.json(200, mobilesuit);
    });
  });
};

exports.updateInventory = function(req, res) {
  UserMobilesuit.findById(req.params.id, function (err, ms) {
    if (err) return handleError(res, err);
    if (!ms) {
      logger.system.info('mobilesuit_ctrl::UpdateInventory not found mobilesuit ' + req.params.id);
      return validationError(res, 422, err);
    }

    var inventory = req.body.newInventory;
    var color = req.body.newColor;
    var buffs = req.body.newBuffs;
    if (!_.isArray(inventory)) { return validationError(res, 422, '不正なインベントリ情報です'); }

    // check the unlock
    // todo: 武器はマスターデータの武器IDをInventoryに入れるのをやめるまではチェックしない
    var unlockIdx = -1;
    if (!(_.isEmpty(color))) {
      var unlockIdx = _.findIndex(ms.unlockedColors, function (uc) { return (uc === color) });
      if (unlockIdx < 0) {
        logger.system.info('mobilesuit_ctrl::UpdateInventory not unlock the color');
        return validationError(res, 422, 'not unlock the color');
      }
    }
    if (!(_.isEmpty(buffs))) {
      unlockIdx = _.findIndex(ms.unlockedBuffs, function (ub) { return (ub === buffs) });
      if (unlockIdx < 0) {
        logger.system.info('mobilesuit_ctrl::UpdateInventory not unlock the buff');
        return validationError(res, 422, 'not unlock the buff');
      }
    }

    ms.inventory = [];
    ms.color = color || '';
    ms.buffs = buffs || '';
    for (var i = 0; i < inventory.length; i++) {
      var data = Master.equipments.object[inventory[i]];
      if (data) {
        if (data.weaponSlotOrder !== i) {
          logger.system.info('mobilesuit_ctrl::UpdateInventory invalid slot ' + inventory[i] + ' ' + data.weaponSlotOrder + ' ' + i);
          //return validationError(res, 422, 'invalid weapon slot');
        }
        ms.inventory.push(inventory[i]);
      }
    }
    ms.save(function(err) {
      if (err) return validationError(res, 422, err);
      logger.system.info('mobilesuit_ctrl::UpdateInventory success');
      res.send(200);
    });
  });
};

// Deletes a user/mobilesuit from the DB.
exports.destroy = function(req, res) {
  UserMobilesuit.findById(req.params.id, function (err, mobilesuit) {
    if(err) { return handleError(res, err); }
    if(!mobilesuit) { return res.send(404); }

    User.findById(req.user._id, function (err, user) {
      if (err) { return handleError(res, err); }
      if (!user) { return res.send(404); }

      // デッキに入っていたら廃棄できなくしておく。
      // ObjectId同士の比較には、.equals()を用いる必要があるので _.contains は使えない。
      // ↑デッキ配列の型がNumberになったので、_.containsを使うようにした
      var inDeck = _.contains(user.deck, mobilesuit._id);
      if (inDeck) {
        return validationError(res, 422, 'デッキに含まれています');
      }

      mobilesuit.remove(function(err) {
        if(err) { return handleError(res, err); }
        return res.send(204);
      });
    });
  });
};

// add a mobilesuit to the specific
exports.addMobileSuit = function(userId, msId, inventory, unlockedWeapons, next) {
  Serials.getNewSerial('serial_mobilesuit', function (err, serial) {
    if (err || serial <= 0) {
      logger.system.info('mobilesuit_ctrl::addMobileSuit cant create mobilesuit, invalid serial');
      next(err, null);
    }

    logger.system.info('mobilesuit_ctrl::addMobileSuit msid:' + msId + ' serial:' + serial);
    Master.mobilesuits.findById(msId, function (err, ms) {
      if (err) { return next(err, null); }
      if (!ms) { return next(null, null); }

      UserMobilesuit.create({
        _id: serial,
        userId: userId,
        msId: msId,
        DisplayName: ms.DisplayName,
        inventory: inventory,
        unlockedWeapons: unlockedWeapons,
        exp: 0
      },
      function(err, mobilesuit) {
        if (err) { return next(err, null); }
        return next(null, mobilesuit);
      });
    })
  });
};

function handleError(res, err) {
  return res.send(500, err);
}
