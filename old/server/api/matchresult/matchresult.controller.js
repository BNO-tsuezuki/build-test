'use strict';

var _ = require('lodash');
var async = require('async');
var User = require('../user/user.model')
var UserStats = require('../user/stats/stats.model');
var MobileSuitStats = require('../user/stats/mobilesuitstats.model');
var UserMobilesuit = require('../user/mobilesuit/mobilesuit.model');
var Master = require('../../config/master');
var fs = require('fs');
var logger = require('../../logger');


// ユーザー1人あたりの、報酬付与情報(クライアントへの通知用)
var userRewardInfo = function() {
  var F = function () {};
  F.prototype = userRewardInfo.prototype;
  return new F;
};
userRewardInfo.prototype = {
  userId : 0,
  gp_shift : 0,
  gp_kill : 0,
  gp_damage : 0,
  gp_medal_get : 0,
  exp_shift : 0,
  exp_kill : 0,
  exp_damage : 0,
  exp_medal_get : 0,

  // クライアントへJSONとしてオブジェクトを返す際、
  // prototypeプロパティは無視されてしまうようなので対策
  toJSON: function() {
    var tmp = {};
    for (var key in this) {
      if (typeof this[key] !== 'function')
        tmp[key] = this[key];
    }
    return tmp;
  }
};


var validationError = function(res, statusCode, err) {
  logger.system.info('matchresult_ctrl:: '+ err);
  return res.status(statusCode).send(err);
};


// 指定ユーザーにGPを付与する
var dealGamePoint = function(taskNext, cfg, req, user, record, rewardInfo) {
    // 勝敗補正
    rewardInfo.gp_shift = cfg.baseGP;
    if (parseInt(record.winner, 10) > 0) {
      rewardInfo.gp_shift += cfg.winGPBonus;
    }
    // メダル獲得補正
    if (parseInt(record.isMedalGet, 10) > 0) {
      rewardInfo.gp_medal_get = cfg.haveMedalGPBonus;
    }
    // 付与
    var total = rewardInfo.gp_shift + rewardInfo.gp_medal_get;
    if (total < 0) total = 0;
    var _gamePoint = user.gamePoint;
    user.gamePoint = parseInt(user.gamePoint, 10) + total;
    user.save(function(err) {
      if (err || !user) {
        logger.system.info("WARN: matchresult_ctrl: cant update user, invalid id, _id=" + record.userID);
        return taskNext(err);
      }
      logger.system.info('matchresult_ctrl: gamepoint update [' + user.name + '] ' + _gamePoint + ' + ' + total + ' -> ' + user.gamePoint);
      return taskNext();
    });
};

// 指定ユーザーのデッキ内のMSに経験値を付与する
var dealExp = function(taskNext, cfg, req, user, record, rewardInfo) {
  // 勝敗補正
  rewardInfo.exp_shift = cfg.baseExp;
  if (parseInt(record.winner, 10) > 0) {
    rewardInfo.exp_shift += cfg.winExpBonus;
  }
  // メダル獲得補正
  if(parseInt(record.isMedalGet, 10) > 0) {
    rewardInfo.exp_medal_get = cfg.haveMedalExpBonus;
  }
  var total = rewardInfo.exp_shift + rewardInfo.exp_medal_get;
  if (total < 0) total = 0;

  async.each(user.deck, function (deck, dealExpMSNext) {
    UserMobilesuit.findById(deck, function (err, ms) {
      if (err || !ms) {
        logger.system.info('WARN: matchresult_ctrl: cant found mobilesuit, invalid id, _id=' + deck);
        return dealExpMSNext(err);
      }
      // 付与
      var _exp = ms.exp;
      ms.exp = parseInt(ms.exp, 10) + total;
      ms.save(function (err) {
        if (err) {
          logger.system.info("WARN: matchresult_ctrl: cant update usermobilesuits, invalid id, _id=" + deck);
          return dealExpMSNext(err);
        }
        logger.system.info('matchresult_ctrl: exp update [' + user.name + ',' + ms._id + ',' + ms.msId + '] ' + _exp + ' + ' + total + ' -> ' + ms.exp);
        dealExpMSNext();
      });
    });
  },
  function dealExpMSComplete(err) {
    taskNext(err);
  });
};

// 指定ユーザーの戦績を更新
var updateStats = function(taskNext, cfg, req, user, record, rewardInfo) {

  UserStats.findById(user._id, function(err, stats) {
    if (err) {
      logger.system.info('matchresult_ctrl: (1) ' + err);
      return taskNext(err);
    }

    // なかったら作る
    if (!stats) {
      stats = new UserStats();
      stats._id = record.userID;
    }

    // 解析&更新
    if (parseInt(record.winner, 10) > 0) {
      stats.win   = parseInt(stats.win, 10) + 1;
    } else {
      stats.lose  = parseInt(stats.lose, 10) + 1;
    }

    stats.time              += parseInt(record.elapsedTime, 10);
    stats.finalblows        += parseInt(record.finalblows, 10);
    stats.eliminations      += parseInt(record.eliminations, 10);
    stats.death             += parseInt(record.deaths, 10);
    stats.damage_given      += parseInt(record.totalgivenDamage, 10);
    stats.damage_taken      += parseInt(record.totaltakenDamage, 10);
    stats.conquered_seconds += parseInt(record.conqueredSeconds, 10);
    stats.conquered_times   += parseInt(record.conqueredTimes, 10);
    stats.maintain_seconds  += parseInt(record.maintainSeconds, 10);


    if (record.killstreak > stats.killstreak) {
      stats.killstreak = record.killstreak;
    }

    stats.recovery += parseInt(record.countRecovery, 10);
    stats.spotted  += parseInt(record.countSpotted, 10);

    // 保存
    stats.save(function(err, record) {
      if (err || !record) {
        logger.system.info('matchresult_ctrl: (2) ' + err);
        return taskNext(err);
      }
      logger.system.info('matchresult_ctrl: [' + stats._id + '] userstats saved.');
      taskNext();
    });
  });
};

// (#86479) MSの戦績を更新
var updateMobileSuitStats = function(taskNext, cfg, req, user, record, rewardInfo) {
  var already = [];

  // 該当ユーザーが使用したMS分ループ
  async.eachSeries(record.mobilesuitStats, function(stats_recv, msNext) {

    logger.system.info( '# mss : ' + user._id + ' / ' + stats_recv.msId );

    // ユーザー所有のMS戦績を探す
    MobileSuitStats.findOne({$and:[{userId: user._id}, {msId: stats_recv.msId}]}, function(err, stats_db) {
      if (err) {
        logger.system.info('matchresult_ctrl: (mss1) ' + err);
        return msNext(err);
      }

      // なかったら作る
      if (!stats_db) {
        stats_db = new MobileSuitStats();
        stats_db.userId  = user._id;
        stats_db.msId = stats_recv.msId;
      } else {
      }

      // - 解析&更新 -
      if (already.indexOf(stats_recv.msId) < 0) {
        if (parseInt(record.winner, 10) > 0) {
          stats_db.win   = parseInt(stats_db.win, 10) + 1;
        } else {
          stats_db.lose  = parseInt(stats_db.lose, 10) + 1;
        }
        already.push(stats_recv.msId);
      }
      //logger.system.info('# already : ' + already);

      stats_db.sortie             += 1;
      stats_db.time               += parseInt(stats_recv.lifeSeconds, 10);
      stats_db.finalblows         += parseInt(stats_recv.finalblows, 10);
      stats_db.eliminations       += parseInt(stats_recv.eliminations, 10);
      stats_db.death              += parseInt(stats_recv.deaths, 10);
      stats_db.damage_given       += parseInt(stats_recv.givenDamage, 10);
      stats_db.damage_taken       += parseInt(stats_recv.takenDamage, 10);
      stats_db.damage_guard       += parseInt(stats_recv.guardDamage, 10);
      stats_db.conquered_seconds  += parseInt(stats_recv.conqueredSeconds, 10);
      stats_db.conquered_times    += parseInt(stats_recv.conqueredTimes, 10);
      stats_db.maintain_seconds   += parseInt(stats_recv.maintainSeconds, 10);
      stats_db.recovery           += parseInt(stats_recv.countRecovery, 10);
      stats_db.spotted            += parseInt(stats_recv.countSpotted, 10);
      stats_db.skill_use          += parseInt(stats_recv.countSkillUse, 10);

      if (stats_recv.eliminations > stats_db.killstreak) {
        stats_db.killstreak = stats_recv.eliminations;
      }

      if (parseInt(stats_recv.lifeSeconds, 10) > stats_db.lifespan_longest) {
        stats_db.lifespan_longest = parseInt(stats_recv.lifeSeconds, 10);
      }


      for (var i = 0, len_i = stats_recv.weaponStats.length; i < len_i; ++i) {
        var found = false;
        for (var j = 0, len_j = stats_db.damage_source.length; j < len_j; ++j) {
          if (stats_db.damage_source[j].name == stats_recv.weaponStats[i].Name) {
            found = true;
            break;
          }
        }


        if (!found) {
          var ds = Object();

          ds["name"]           = stats_recv.weaponStats[i].Name;
          ds["kill"]           = stats_recv.weaponStats[i].kills;
          ds["damage_given"]   = stats_recv.weaponStats[i].givenDamage;
          ds["reload"]         = stats_recv.weaponStats[i].countReload;
          ds["emit_count"]     = stats_recv.weaponStats[i].countEmit;
          ds["hit_count"]      = stats_recv.weaponStats[i].countHit;
          ds["headshot_count"] = stats_recv.weaponStats[i].countHeadshot;
          ds["backshot_count"] = stats_recv.weaponStats[i].countBackshot;

          stats_db.damage_source.push(ds);
        } else {
          stats_db.damage_source[j].kill           += parseInt(stats_recv.weaponStats[i].kills, 10);
          stats_db.damage_source[j].damage_given   += parseInt(stats_recv.weaponStats[i].givenDamage, 10);
          stats_db.damage_source[j].reload         += parseInt(stats_recv.weaponStats[i].countReload, 10);
          stats_db.damage_source[j].emit_count     += parseInt(stats_recv.weaponStats[i].countEmit, 10);
          stats_db.damage_source[j].hit_count      += parseInt(stats_recv.weaponStats[i].countHit, 10);
          stats_db.damage_source[j].headshot_count += parseInt(stats_recv.weaponStats[i].countHeadshot, 10);
          stats_db.damage_source[j].backshot_count += parseInt(stats_recv.weaponStats[i].countBackshot, 10);
        }
      }

      logger.system.info(stats_recv);
      //logger.system.info(stats_db);

      // - 保存 -
      stats_db.save(function(err, record) {
        if (err || !record) {
          logger.system.info('matchresult_ctrl: (mss2) ' + err);
          return msNext(err);
        }
        logger.system.info('matchresult_ctrl: [' + stats_db._id + '] mobilesuitstats saved.');
        msNext();
      });

    });

  },
  // MSループ完了
  function complete(err) {
    taskNext(err);
  });

};

// α4用リザルト報酬付与ロジック
exports.dealOutReward = function(req, res) {
  // TODO: ゲームモードによって読み込むリザルト報酬を変えられるようになっているといいかも
  Master.resultReward.findById('Normal', function rewardSettingsFind(err, cfg) {
    if (err || !cfg) {
      logger.system.info('WARN matchresult_ctrl: notfound settings');
      return validationError(res, 404, err);
    }

    var records = _.filter(req.body.records, function(record) {
      return record.userID !== '';
    });
    if (!records.length) {
      return res.send(400);
    }
    logger.system.info('matchresult_ctrl: finish match [' + req.body.gameMode + ']')

    var obj_res = Object();
    obj_res.rewards = Array();

    // 参加ユーザー1人ずつにGPとEXPの付与、戦績の更新を行う
    var tasks = [dealGamePoint, dealExp, updateStats, updateMobileSuitStats];
      if (parseInt(req.body.updateGameStats, 10) == 0) {
      tasks = [dealGamePoint, dealExp];
    }

    async.eachSeries(records, function(record, recordNext) {
      User.findById(record.userID, function(err, user) {
        if (err || !user) {
          logger.system.info( 'WARN: matchresult_controller: notfound user, invalid _id=' + record.userID);
          return recordNext(err);
        }
        // クライアント返答用の報酬付与情報インスタンス生成
        var rewardInfo = userRewardInfo();
        rewardInfo.userId = record.userID;

        async.eachSeries(tasks, function(task, taskNext) {
          task(taskNext, cfg, req, user, record, rewardInfo);
        },
        function tasksComplete (err) {
          if (err) {
            return recordNext(err);
          }
          // 報酬付与情報を返答用配列に追加
          obj_res.rewards.push(rewardInfo);
          recordNext();
        });
      });
    },
    function dealComplete(err) {
      if (err) {
        return validationError(res, 422, err);
      }
      logger.system.info('matchresult_ctrl: (' + obj_res.rewards.length + 'records)');
      _.forEach(obj_res.rewards, function(d) {
        logger.system.info('[' + d.userId + '] GP(shift: '  + d.gp_shift + ' medal:'  + d.gp_medal_get + ')');
        logger.system.info('[' + d.userId + '] EXP(shift: ' + d.exp_shift + ' medal:' + d.exp_medal_get + ')');
      });
      // 最終返答
      res.json(200, obj_res);
    });
  });
}


// ==========
// α4前の旧ロジック。最終的に必要なくなれば削除する
exports.matchresult = function(req, res) {
  var cfg = JSON.parse(fs.readFileSync('server/config/reward.json', 'utf-8'));

  var arr_res = Array();
  // ※arr_resへの格納順を固定したかったので一部のループをeachSeriesに変更しました。

  var records = _.filter(req.body.records, function (record) {
    return record.userID !== '';
  });
  if (!records.length) {
    return res.send(400);
  }

  // 参戦ユーザ全員にゲームポイントを付与する
  var taskGamePoint = function(next) {
    var gamePoint = 0;
    async.eachSeries(records, function (record, next) {
      User.findById(record.userID, function (err, user) {
        if (err || !user) {
          logger.system.info("WARN: matchresult_controller: cant found user, invalid id, _id=" + record.userID);
          next();
        } else {
          var obj = Object();
          obj["userId"] = record.userID;

          // (初期値)
          obj["exp_shift"] = 0;
          obj["exp_kill"] = 0;
          obj["exp_damage"] = 0;

          // 支給額算出
          if(parseInt(record.winner, 10) > 0)
            gamePoint = cfg.gp_win_team;
          else
            gamePoint = cfg.gp_lose_team;

          obj["gp_shift"] = gamePoint;

          obj["gp_kill"] = parseInt(record.eliminations, 10) * cfg.gp_rate_kill;
          gamePoint += obj["gp_kill"];
          obj.gp_damage = parseInt(record.totalgivenDamage, 10) * cfg.gp_rate_damage;
          gamePoint += obj["gp_damage"];

          logger.system.info( "#(mr1) id : " + record.userID );

          // 付与
          var _gamePoint = user.gamePoint;
          user.gamePoint = parseInt(user.gamePoint, 10) + gamePoint;
          user.save(function(err) {
            if (err || !user) {
              logger.system.info("WARN: matchresult_controller: cant update user, invalid id, _id=" + record.userID);
            } else {
              logger.system.info('matchresult_controller: gamepoint update [' + user.name + '] ' + _gamePoint + ' + ' + gamePoint + ' -> ' + user.gamePoint);
            }
            next();
          });

          arr_res.push(obj);
        }
      });
    }, function (err) {
      next(err);
    });
  };

  // 参戦ユーザの戦績を更新(t4-kobayashi:user/statsから統合)
  var taskStats = function(next) {
    async.each(records, function (record, next) {
      if(!record.userID) return next();
      if(record.userID.length <= 0) return next();

      // db.userstatsから任意のユーザーを探す
      UserStats.findById(record.userID, function(err, stats) {
        if (err) {
          logger.system.info('matchresult: (1) ' + err);
          return next();
        }

        // なかったら作る
        if (!stats) {
          stats = new UserStats();
          stats._id = record.userID;
        }


        // 解析&更新
        if(parseInt(record.winner, 10) > 0)
          stats.win   = parseInt(stats.win, 10) + 1;
        else
          stats.lose  = parseInt(stats.lose, 10) + 1;

        stats.time         += parseInt(req.body.matchpassedTime, 10);
        stats.finalblows   += parseInt(record.finalblows, 10);
        stats.eliminations += parseInt(record.eliminations, 10);
        stats.death        += parseInt(record.deaths, 10);
        stats.damage_given += parseInt(record.totalgivenDamage, 10);
        stats.damage_taken += parseInt(record.totaltakenDamage, 10);

        if(record.killstreak > stats.killstreak)
          stats.killstreak = record.killstreak;

        stats.recovery += parseInt(record.countRecovery, 10);
        stats.spotted += parseInt(record.countSpotted, 10);


        // 保存
        stats.save(function(err, record) {
          if (err) {
            logger.system.info('matchresult: (2) ' + err);
            return next();
          }
          logger.system.info('matchresult: [' + stats._id + '] userstats saved.');
          next();
        });

      });
    },
    // ループ完了
    function(err) {
      next(err);
    });
  };


  // 参戦ユーザ全員のデッキ内のMSに経験値を付与する
  var taskExp = function(next) {
    var exp = 0;
    var idx = 0;

    async.eachSeries(records, function (record, next) {
      User.findById(record.userID, function (err, user) {
        if (err || !user) {
          logger.system.info("WARN: matchresult_controller: cant found user, invalid id, _id=" + record.userID);
          next();
        } else {
          async.each(user.deck, function (deck, next) {
            UserMobilesuit.findById(deck, function (err, ms) {
              if (err || !ms) {
                logger.system.info("WARN: matchresult_controller: cant found mobilesuit, invalid id, _id=" + deck);
                next();
              } else {
                // 支給量算出
                if(parseInt(record.winner, 10) > 0)
                  exp = cfg.exp_win_team;
                else
                  exp = cfg.exp_lose_team;

                logger.system.info( "#(mr2) id : " + record.userID + ", idx : " + idx + ", arr_res[idx] : " + arr_res[idx] );

                var shift = exp;
                var damage = 0;
                var eliminations = 0;

                eliminations = parseInt(record.eliminations, 10) * cfg.exp_rate_kill;
                exp += eliminations;
                damage = parseInt(record.totalgivenDamage, 10) * cfg.exp_rate_damage;
                exp += damage;

                if(idx < arr_res.length ) {
                  if("exp_shift" in arr_res[idx])
                    arr_res[idx]["exp_shift"] = shift;

                  if("exp_kill" in arr_res[idx])
                    arr_res[idx]["exp_kill"] = eliminations;

                  if("exp_damage" in arr_res[idx])
                    arr_res[idx]["exp_damage"] = damage;

                } else {
                  logger.system.info('#(mr3) : ' + idx);
                }

                // 付与
                var _exp = ms.exp;
                ms.exp = parseInt(ms.exp, 10) + exp;
                ms.save(function (err) {
                  if (err) {
                    logger.system.info("WARN: matchresult_controller: cant update usermobilesuits, invalid id, _id=" + deck);
                  } else {
                    logger.system.info('matchresult_controller exp update [' + user.name + ',' + ms._id + ',' + ms.msId + '] ' + _exp + ' + ' + exp + ' -> ' + ms.exp);
                  }
                  next();
                });
              }
            });
          }, function (err) {
            idx++;
            next(err);
          });
        }
      });
    }, function (err) {
      next(err);
    });
  };

  logger.system.info('matchresult_controller finish match [' + req.body.gameMode + ']')
  var tasks = [taskGamePoint, taskStats, taskExp];
  async.eachSeries(tasks, function(task, next) {
    task(next);
  }, function(err) {
    if (err) return validationError(res, 422, err);

    var obj_res = Object();
    obj_res["rewards"] = arr_res;

    logger.system.info('matchresult_controller: respond ' + obj_res + ', (' + arr_res.length + 'records)');

    var i = 0, len = arr_res.length;
    for(i = 0; i < len; i++)
    {
      logger.system.info("#(mr4) : [" + arr_res[i]["userId"] + "] " + (arr_res[i]["exp_shift"] + arr_res[i]["exp_kill"] + arr_res[i]["exp_damage"]) + "exp, " + (arr_res[i]["gp_shift"] + arr_res[i]["gp_kill"] + arr_res[i]["gp_damage"]) + "gp");
    }

    // 最終返答
    res.json(200, obj_res);

  });
}

function handleError(res, err) {
  return res.send(500, err);
}
