'use strict';

var _ = require('lodash');
var async = require('async');
var UserMobilesuit = require('../mobilesuit/mobilesuit.model');
var User = require('../user.model');
var UserStats = require('./stats.model');
var MobileSuitStats = require('./mobilesuitstats.model');
var Serials = require('../../serials/serials.model');
var Master = require('../../../config/master');
var logger = require('../../../logger');

var validationError = function(res, statusCode, err) {
  logger.system.info('stats_ctrl:: '+ err);
  return res.status(statusCode).send(err);
};

exports.view = function(req, res) {
  logger.system.info('# stats_ctrl::view : ' + req.params.id);

  // one
  if(req.params.id)
  {
    UserStats.findById(req.params.id, function (err, userstats) {
      if(err) { return handleError(res, err); }

      User.findById(req.params.id, function (err, user) {
        if(err) return res.status(200).json(userstats);

        // ユーザー名を追加
        if(user)
          userstats["name"] = user._id;
        else
          userstats["name"] = '';

        return res.status(200).json(userstats);
      });
    });
  }
  // multi
  else
  {
    logger.system.info('# stats_ctrl::view : multi');

    UserStats.find({}, {__v:0}, function (err, userstats) {
      if(err) { return handleError(res, err); }

      async.each(userstats, function(stat, next) {
        User.findById(stat._id, function (err, user) {
          if (err) {
            next(err);
            logger.system.info("# not found : " + stat._id);
            return;
          }

          // ユーザー名を追加
          if (user)
            stat["name"] = user.name;
          else
            stat["name"] = '';


          // (#86479)
          if (user)
          {
            MobileSuitStats.find({userId: stat._id}, {__v:0}, function(err, mssl) {
              if (err) {
                logger.system.info('# mss err : ' + err);
                return next();
              }

              stat["mobilesuit_stat"] = mssl;

              //logger.system.info('# mss : ' + stat);

              next();
            });
          } else {
            next();
          }


        });
      }, function complete(err) {
        return res.status(200).json(userstats);
      });
    });
  }
};


// (get) user stats
exports.show = function(req, res) {
  var userId = req.user._id;
  UserStats.findById(userId, function(err, stats) {
    res.json(200, stats);
  });
};


// (post) user stats
exports.create = function(req, res) {
  logger.system.info('# reportStats');
  var winnerTeam = parseInt(req.body.winner_team, 10);

  logger.system.info( req.body );

  // ユーザー分ループ
  async.each(req.body.players, function (user, next) {
    if(!user.userID) return;
    if(user.userID.length <= 0) return;

    // db.userstatsから任意のユーザーを探す
    UserStats.findById(user.userID, function(err, stats) {
      if (err)
      {
        logger.system.info('userstats: (1) ' + err);
        return;
      }

      // なかったら作る
      if (!stats)
      {
        logger.system.info('# 新規');

        stats = new UserStats();
        stats._id = user.userID;
      }
      else
      {
        logger.system.info('# 既存');
      }

      console.log('# typeof : ' + typeof(stats.eliminations));

      // 解析&更新
      if(parseInt(user.winner, 10) > 0)
        stats.win   = parseInt(stats.win, 10) + 1;
      else
        stats.lose  = parseInt(stats.lose, 10) + 1;

      stats.time         += parseInt(req.body.matchpassedTime, 10);
      stats.finalblows   += parseInt(user.finalblows, 10);
      stats.eliminations += parseInt(user.eliminations, 10);
      stats.death        += parseInt(user.deaths, 10);
      stats.damage_given += parseInt(user.totalgivenDamage, 10);
      stats.damage_taken += parseInt(user.totaltakenDamage, 10);


      if(user.killstreak > stats.killstreak)
        stats.killstreak = user.killstreak;
      stats.recovery += parseInt(user.countRecovery, 10);
      stats.spotted += parseInt(user.countSpotted, 10);

      logger.system.info( "# ---- ---- ---- ----" );
      logger.system.info( stats );

      // 保存
      stats.save(function(err, user) {
        if (err)
        {
          logger.system.info('userstats: (2) ' + err);
          return;
        }
        logger.system.info('userstats: saved');
      });

    });
  },
  // ループ完了
  function complete (err) {
    if (err)
    {
      logger.system.info('userstats: (3) ' + err);
      return res.status(422).send(err);
    }

    logger.system.info( '# complete' );

    return res.status(200);
  });
};

exports.mstable = function(req, res) {
  var table = Object();

  var arr = Master.mobilesuits.array;
  //Master.mobilesuits.find({}, {Name:1, displayName:1}, function (err, records) {
  for (var i = 0, len = arr.length; i < len; ++i) {
    table[arr[i].Name] = arr[i].displayName;
  }

  //logger.system.info(table);

  res.json(200, table);
};

function handleError(res, err) {
  return res.send(500, err);
}
