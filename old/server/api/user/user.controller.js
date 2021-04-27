'use strict';

var User = require('./user.model');
var UserMobilesuit = require('./mobilesuit/mobilesuit.model');
var UserStats = require('./stats/stats.model');
var passport = require('passport');
var config = require('../../config/environment');
var Master = require('../../config/master');
var jwt = require('jsonwebtoken');
var _ = require('lodash');
var async = require('async');
var mongoose = require('mongoose');
var ms = require('./mobilesuit/mobilesuit.controller');
var DevTreeController = require('./devtree/devtree.controller');
var logger = require('../../logger');


var validationError = function(res, statusCode, err) {
  logger.system.info('user_ctrl:: '+ err);
  return res.status(statusCode).send(err);
};


/**
 * Get list of users
 * restriction: 'admin'
 */
exports.index = function(req, res) {
  User.find({}, '-salt -hashedPassword', function (err, users) {
    if(err) return res.send(500, err);
    res.json(200, users);
  });
};

/**
 * Creates a new user
 */
exports.create = function (req, res, next) {
  var newUser = new User(req.body);
  newUser.provider = 'local';
  newUser.role = 'user';
  newUser.gamePoint = 0;

  // テーブルから初期機体を抽選
  var totalLot = 0;
  _.forEach(Master.init_ms_lot.array, function (msLot) { totalLot += msLot.probability; });
  logger.system.info('user_ctrl::create : ' + newUser.name + ' email: ' + newUser.email + ' initMStotalLot=' + totalLot );

  if(totalLot > 0) {
    var lotNum = _.random(1, totalLot);
    _.forEach(Master.init_ms_lot.array, function (msLot2) {
        totalLot -= msLot2.probability;
        if (totalLot < lotNum) {
          newUser.initialMSId = msLot2.nodeId;
          logger.system.info('user_ctrl::create : init ms=' + newUser.initialMSId + ' lotNum=' + lotNum + ' totalLot=' + totalLot );
          return false; // break the loop
        }
    });
  } else {
    // 抽選不可なので、警告出して固定の機体を指定しておく
    logger.system.info('user_ctrl::create : WARN cant lot initMS, set default GM');
    newUser.initialMSId = 'GM';
  }

  // @note: 大規模テスト専用処理。ユーザーに初期GP配布しておく
  newUser.gamePoint = 1000;

  newUser.save(function(err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res, 404, 'user_ctrl::create : WARN user create failed');

    // @note : 大規模テスト専用処理。配布を止める
    // 初期機体配布
    DevTreeController.dealMobileSuit(user, user.initialMSId, function(err) {
      if (err) {
        // 失敗したら作成した新規ユーザーごと消す
        User.findByIdAndRemove(user._id, function(err, user) {
        });
        return validationError(res, 400, err);
      }
      var token = jwt.sign({_id: user._id }, config.secrets.session, { expiresInMinutes: 60*5 });
      res.json({ token: token });
    });

  });
};

/**
 * Get a single user
 */
exports.show = function (req, res, next) {
  var userId = req.params.id;

  User.findById(userId).lean().exec( function (err, user) {
    if (err) return next(err);
    if (!user) return res.send(404);

    UserMobilesuit.find({ userId: userId }, function (err, mslist) {
        if (err) return next(err);
        if (!mslist) return res.json(404);
        for(var i=0; i < user.deck.length; ++i){
          // デッキ順にMS情報を設定する
          var ms =  _.find(mslist, function(ms){ return ms._id == user.deck[i]; });
          user.deck[i] = ms;
        }

        res.json({"name":user.name, "role":user.role, "deck":user.deck});
    });
  });
};

/**
 * Deletes a user
 * restriction: 'admin'
 */
exports.destroy = function(req, res) {
  User.findByIdAndRemove(req.params.id, function(err, user) {
    if(err) return res.send(500, err);
    return res.send(204);
  });
};

/**
 * Change a users password
 */
exports.changePassword = function(req, res, next) {
  var userId = req.user._id;
  var oldPass = String(req.body.oldPassword);
  var newPass = String(req.body.newPassword);

  User.findById(userId, function (err, user) {
    if(user.authenticate(oldPass)) {
      user.password = newPass;
      user.save(function(err) {
        if (err) return validationError(res, 422, err);
        res.send(200);
      });
    } else {
      res.send(403);
    }
  });
};

/**
 * Add users gamepoint
 */
exports.addGamePoint = function(req, res, next) {
  var userId = req.user._id;
  var gamePoint = parseInt(req.body.addPoint, 10);

  User.findById(userId, function (err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res,404, 'cant add gamepoint not found user');
    var _gamePoint = user.gamePoint;
    user.gamePoint = parseInt(user.gamePoint, 10) + gamePoint;
    user.save(function(err) {
      if (err) return validationError(res, 422, err);
      logger.system.info('user_controller: gamepoint update [' + user.name + '] ' + _gamePoint + '+' + gamePoint + '->' + user.gamePoint);
      res.send(200);
    });
  });
};

/**
 * Update users deck
 */
exports.updateDeck = function(req, res, next) {
  var userId = req.user._id;
  var deck = req.body.newDeck;

  deck = _.filter(deck, function (d) {
    return d !== '';
  });

  if (deck.length === 0) {
    return validationError(res, 422, '少なくとも１機体はセットしてください');
  }

  User.findById(userId, function (err, user) {
    user.deck = _.map(deck, function (d) {
      return d;
    });
    user.save(function(err) {
      if (err) return validationError(res, 422, err);
      res.send(200);
    });
  });
};

/**
 * Get my info
 */
exports.me = function(req, res, next) {
  var userId = req.user._id;
  // @tips Mongoose.Model は Document で、プロパティの追加を許してくれないらしい。
  //       したがって、ModelでなくPlain Objectで受け取るために lean() を使う。
  //       don't ever give out the password or salt
  User.findOne({
    _id: userId
  }, '-salt -hashedPassword').lean().exec(function(err, user) {
    if (err) return next(err);
    if (!user) return validationError(res, 404, 'notfound User');

    if (req.query.type === 'detail') {
      UserMobilesuit.find({ userId: userId }, function (err, mslist) {
        if (err) return next(err);
        if (!mslist) return validationError(res, 404, 'notfound UserDeck');
        user.mslist = mslist; // スキーマ定義には無いけど、送信用にプロパティを追加
        for (var i=0; i<user.deck.length; ++i) {
          // デッキ順にMS情報を設定する
          var ms =  _.find(mslist, function(ms) { return ms._id == user.deck[i]; });
          user.deck[i] = ms;
        }

        logger.system.info( '# user detail(1) : ' + user);
        // - user stats -
        UserStats.findById(userId, function(err, stats) {
          if (err) return res.json(201, user);
          if (!stats) return res.json(201, user);

          user.stats = stats;

          res.json(200, user);

          logger.system.info( '# user detail(2) : ' + user);
        });
        // - user stats -

        //res.json(user);
      });
    } else {
      res.json(user);
    }
  });
};

/**
 * Authentication callback
 */
exports.authCallback = function(req, res, next) {
  res.redirect('/');
};
