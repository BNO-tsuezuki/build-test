'use strict';

var _ = require('lodash');
var async = require('async');
var logger = require('../../logger');
var UserMobilesuit = require('../user/mobilesuit/mobilesuit.model');
var UserStats = require('../user/stats/stats.model');

var sessionList = {};
var waitingList = {};


// マッチング完了に必要な人数(最大値/現在値)
var matchmakingMenberMax = 64;
var matchmakingMenberNum = 64;


// サーバアドレスの取得
var getServerAddress = function(req) {
  return req.body.host + ":" + req.body.port;
};


// セッションがマッチング中か
var isMatchmakingSession = function(session) {
  if (_.has(session, 'gamemode')) {
    return session.gamemode.toLowerCase() === 'matching'.toLowerCase();
  }
  return false;
};


// セッションリストの更新
var updateSessionList = function(req) {
  var session = undefined;

  if (req) {
    var addr = getServerAddress(req);
    if (!_.has(sessionList, addr)) {
      sessionList[addr] = { matching: false };
    }
    session = sessionList[addr];
    session.servername = req.body.servername;
    session.host = req.body.host;
    session.port = req.body.port;
    session.session = req.body.session;
    session.mapname = req.body.mapname;
    session.gamemode = req.body.gamemode;
    session.lastAlive = Date.now();
    logger.system.info('session_ctrl: update session: ' + addr);

    if (!isMatchmakingSession(session) && session.matching) {
      session.matching = false;
    }
  }

  // セッションリストをデバッグログ出力する
  _.forEach(sessionList, function(session, addr) {
    logger.system.debug('session_ctrl: current sessions:   ' + addr);
  });

  // 一定時間更新の無いセッションを削除する
  sessionList = _.omit(sessionList, function(session, addr) {
    if ((session.lastAlive + (60 * 1000)) < Date.now()) {
      logger.system.info('session_ctrl: removed timeout session: ' + addr);
      return true;
    }
    return false;
  });

  return session;
};


// 待機中リストの更新
var updateWaitingList = function(req) {
  var waiting = undefined;

  if (req) {
    var userId = req.user._id;
    if (!_.has(waitingList, userId)) {
      waitingList[userId] = {
        entry: Date.now(),
        name: req.user.name,
        deck: [],
        email: req.user.email,
        remoteAddress: req.connection.remoteAddress,
        isMatched: false,
        matching: false
      };

      UserMobilesuit.find({ userId: userId }, function (err, mslist) {
        if (!err && mslist) {
          for(var i=0; i < req.user.deck.length; ++i){
            var ms =  _.find(mslist, function(ms){ return ms._id == req.user.deck[i]; });
            waitingList[userId].deck.push(ms);
          }
        }
      });
    }
    waiting = waitingList[userId];
    waiting.lastAlive = Date.now();
    logger.system.info('session_ctrl: update waiting entry: ' + waiting.name + ' ' + userId);
  }

  // 待機中リストをデバッグログ出力する
  _.forEach(waitingList, function(waiting, userId) {
    logger.system.debug('session_ctrl: current waiting entries:   ' + waiting.name + ' ' + userId);
  });

  // 一定時間更新の無いユーザを削除する
  waitingList = _.omit(waitingList, function(waiting, userId) {
    waiting.waitingSeconds = (Date.now() - waiting.entry) / 1000;
    if ((waiting.lastAlive + (15 * 1000)) < Date.now()) {
      logger.system.info('session_ctrl: removed timeout waiting entry: ' + waiting.name + ' ' + userId);
      return true;
    }
    return false;
  });

  return waiting;
};


// マッチング実行
var processMatchmaking = function() {

  // 空きセッションを探す
  var find = function(session, addr){
    if (!isMatchmakingSession(session)) {
      return false;	// マッチング中ではない
    }
    if (session.matching) {
      return false;	// マッチング情報が存在する
    }
    return true;
  };
  var addr = _.findKey(sessionList, find);
  var session = _.find(sessionList, find);
  if (!session) {
    return false; // 空きセッションが見つからない
  }

  // マッチング待機中ユーザのリストを作成する
  var matchingList = _.omit(waitingList, function(waiting) {
    return waiting.matching;	// マッチング情報が存在するユーザーは削除
  });

  // マッチングが確定条件をチェックする
  if (_.size(matchingList) < matchmakingMenberNum) {
    return false;
  }

  // マップとゲームモードを決める
  var mapname = _.sample(['LV002_mb_Desert_Sunny_c']);
  var gamemode = _.sample(['Conquest']);

  // 確定したのでリストにマッチング情報を設定する
  var matching = {
    mapname: mapname,
    gamemode: gamemode,
  };

  // 参加者に対してマッチング情報設定と超仮レートによるチーム振り分けを行う
  async.forEachOf(matchingList, function(waiting, userId, next) {
    waiting.isMatched = true;
    waiting.matching = _.clone(matching);
    waiting.matching.addr = addr;
    waiting.matching.rate = 0;

    // 戦績テーブルから、勝利数、与ダメ、撃破数を取得
    UserStats.findById(userId, function(err, stats){
      if (err) {
        logger.system.info('session_ctrl: cant get stats, invalid userID');
        return next();
      }
      if (!stats) {
        logger.system.info('session_ctrl: cant get stats');
        return next();
      }
      // TODO: 超仮レート算出
      var rate = (stats.win * 5000) + (stats.damage_given * 0.4) + (stats.eliminations * 600);
      waiting.matching.rate = rate;
      next();
    });
  },
  function matchingListSearchCompleted(err) {
    // 参加者をRateで降順ソートしてチームを決める
    matchingList = _.sortBy(matchingList, function(d) { return d.matching.rate; });
    matchingList.reverse();
    var teamIdx = 1;
    async.eachSeries(matchingList, function(w, next) {
      teamIdx = 1 - teamIdx;
      w.matching.team = teamIdx;
      logger.system.info('session_ctrl: name = ' + w.name + ' team = ' + w.matching.team + ' rate = ' + w.matching.rate);
      next();
    });
  });
  session.matching = matching;

  // コマンドで人数の変更が有ったかもしれないので一旦リセット
  matchmakingMenberNum = matchmakingMenberMax;

  return true;
};


// 強制マッチング実行
var forceMatchmaking = function(addr, users, mapname, gamemode, demoRecordType, startupTime, restartNum, updateStats) {

  // 対象セッションを探す
  var find = function(session, _addr) {
    if (_addr !== addr) {
      logger.system.error('session_ctrl: force matchmaking failed: session ' + addr + ' not found');
      return false;	// 指定のセッションではない
    }
    if (!isMatchmakingSession(session)) {
      logger.system.error('session_ctrl: force matchmaking failed: session ' +  addr + ' not in matchmaking');
      return false;	// マッチング中ではない
    }
    if (session.matching) {
      logger.system.error('session_ctrl: force matchmaking failed: session ' +  addr + ' already matched');
      return false;	// マッチング情報が存在する
    }
    return true;
  };
  var session = _.find(sessionList, find);
  if (!session) {
    return false; // セッションが見つからない
  }

  // 対象セッション、ユーザにマッチング情報を設定する
  var matching = {
    mapname: mapname,
    gamemode: gamemode,
    demorecordtype: demoRecordType,
    startuptime: startupTime,
    restartnum: restartNum,
    updatestats: updateStats,
  };
  session.matching = matching;

  _.forEach(users, function(user) {
    var waiting = _.find(waitingList, function(waiting, userId) {
      if (user.userId === userId) {
        if (!waiting.matching) {
          return true;
        }
      }
      return false;
    });
    if (waiting) {
      waiting.isMatched = true;
      waiting.matching = _.clone(matching);
      waiting.matching.addr = addr;
      waiting.matching.team = 0;
      if (user.selectedTeam === 'B') {
        waiting.matching.team = 1;
      } else if (user.selectedTeam === 'S') {
        waiting.matching.team = -1;
      }
      logger.system.info('session_ctrl: set matching info: ' + waiting.name + ' ' + user.userId + ' team is ' + waiting.matching.team);
    }
  });

  return true;
};


// セッション一覧
exports.sessions = function(req, res) {

  // セッションリストの更新
  updateSessionList(false);

  var resList = _.pick(sessionList, function(session, addr) {
    if (isMatchmakingSession(session)) {
      return true/*いまは全部表示するfalse*/;
    }
    return true;
  });
  resList.hosts = _.keys(resList);
  return res.json(200, resList);
};


// 待機中リスト
exports.waitings = function(req, res) {

  // 待機中リストの更新
  updateWaitingList(false);

  return res.json(200, waitingList);
};


// セッション開始
exports.sessionStart = function(req, res) {
  var addr = getServerAddress(req);
  if (sessionList[addr]) {
    if (!isMatchmakingSession(sessionList[addr])) {
      logger.system.error('session_ctrl: session already started: ' +  addr);
      return handleError(res, 'session ' + addr + ' is already started');
    }
  }

  // セッションリストの更新
  var session = updateSessionList(req);

  logger.system.info('session_ctrl: session started: ' +  addr + ' ' + session.mapname + '?game=' + session.gamemode);
  return res.send(200);
};


// セッション終了
exports.sessionEnd = function(req, res) {
  var addr = getServerAddress(req);
  if (!sessionList[addr]) {
    logger.system.error('session_ctrl: session not exist: ' +  addr);
    return handleError(res, 'session ' + addr + ' is not exist.');
  }

  // セッションリストから削除
  delete sessionList[addr];

  logger.system.info('session_ctrl: session end: ' + addr);
  return res.send(200);
};


// セッション継続
exports.sessionKeepalive = function(req, res) {
  var addr = getServerAddress(req);
  logger.system.info(addr);
  if (!sessionList[addr]) {
    logger.system.error('session_ctrl: session not exist: ' +  addr);
    return handleError(res, 'session ' + addr + ' is not exist');
  }

  // セッションリストの更新
  var session = updateSessionList(req);

  logger.system.info('session_ctrl: session keepalive: ' +  addr + ' ' + session.mapname + '?game=' + session.gamemode);
  return res.send(200);
};


// マッチング開始
exports.matchmakingStart = function(req, res) {
  var addr = getServerAddress(req);

  // 待機中リストの更新
  updateWaitingList(false);

  // セッションリストの更新
  var session = updateSessionList(req);

  // マッチング実行
  processMatchmaking();

  // マッチングが確定していないのでAccepted(202)を返す
  if (!session.matching) {
    logger.system.info('session_ctrl: session matchmaking not match: ' +  addr);
    return res.send(202);
  }

  // マッチングが確定したのでOK(200)を返す
  var matching = {
    mapname: session.matching.mapname,
    gamemode: session.matching.gamemode,
    demorecordtype: session.matching.demorecordtype,
    startuptime: session.matching.startuptime,
    restartnum: session.matching.restartnum,
    updatestats: session.matching.updatestats,
  };

  logger.system.info("session_ctrl: session matchmaking MATCHED!: " +  addr + ' ' + matching.mapname + '?game=' + matching.gamemode);
  return res.json(200, matching);
};


// マッチング参加
exports.matchmakingEntry = function(req, res) {
  var userId = req.user._id;;

  // 待機中リストの更新
  var waiting = updateWaitingList(req);

  // セッションリストの更新
  updateSessionList(false);

  // マッチングが確定しセッションが開始したらOK(200)を返す
  if (waiting.matching) {
    var addr = waiting.matching.addr;
    var session = sessionList[addr];
    if (session && !isMatchmakingSession(session)) {
      logger.system.info('session_ctrl: session matchmaking entry matched: ' + waiting.name + ' ' + userId + ' for ' + waiting.waitingSeconds + ' seconds');

      // 待機中リストから削除
      delete waitingList[userId];

      var resList = { addr: session };
      resList.hosts = _.keys(resList);
      resList.addr.team = waiting.matching.team;
      resList.addr.remoteAddress = waiting.remoteAddress;
      return res.json(200, resList);
    }
  }
  logger.system.info('session_ctrl: session matchmaking entry: ' + waiting.name + ' ' + userId + ' for ' + waiting.waitingSeconds + ' seconds');

  // マッチングが確定していないのでAccepted(202)を返す
  var resList = _.clone(waitingList);
  resList.users = _.keys(resList);
  return res.json(202, resList);
};


// マッチング離脱
exports.matchmakingCancel = function(req, res) {
  var userId = req.user._id;;
  if (!waitingList[userId]) {
    return handleError(res, 'session_ctrl: user ' + userId + ' is not exist.');
  }

  // 待機中リストの更新
  var waiting = updateWaitingList(req);
  logger.system.info('session_ctrl: session matchmaking cancel: ' + waiting.name + ' ' + userId);

  // 待機列から外す
  delete waitingList[userId];

  return res.send(200);
};


// マッチング強制実行
exports.matchmakingForce = function(req, res) {
  var session = req.body.session;
  var users = req.body.users;
  var mapname = req.body.mapname;
  var gamemode = req.body.gamemode;
  var demoRecordType = req.body.demorecordtype;
  var startupTime = parseInt(req.body.startuptime, 10);
  var restartNum = parseInt(req.body.restartnum, 10);
  var updateStats = parseInt(req.body.updatestats, 10);
  if (!session) {
    return handleError(res, 'invalid session');
  }
  if (!users || !_.size(users)) {
    return handleError(res, 'invalid user');
  }
  if (!mapname) {
    return handleError(res, 'invalid map');
  }
  if (!gamemode) {
    return handleError(res, 'invalid game mode');
  }
  if (isNaN(startupTime) || startupTime < 0) {
    return handleError(res, 'invalid startup time');
  }
  if (isNaN(restartNum) || restartNum < 0) {
    return handleError(res, 'invalid restart num');
  }
  if (isNaN(updateStats)) {
    return handleError(res, 'invalid update stats');
  }

  logger.system.info("session_ctrl: session matchmaking force: " + session.addr + " " + mapname + '?game=' + gamemode + ' demo' + demoRecordType + ' startup' + startupTime + ' restart' + restartNum + ' stats' + updateStats);
  _.forEach(users, function(user, index) {
    logger.system.info("session_ctrl: session matchmaking force:   " + user.name + " team is " + user.selectedTeam);
  });

  // マッチング強制実行
  if (!forceMatchmaking(session.addr, users,  mapname, gamemode, demoRecordType, startupTime, restartNum, updateStats)) {
    return handleError(res, 'session ' + session.addr + ' is not available');
  }

  // マッチングが成功したのでOK(200)を返す
  logger.system.info("session_ctrl: session matchmaking force succeed: " + session.addr);
  return res.send(200);
}


// マッチング完了に必要な人数の変更(デバッグ用)
exports.matchmakingChangeMemberMax = function(req, res) {
  var membersMax = parseInt(req.body.membersmax, 10);
  if (membersMax <= 0) {
    membersMax = 1;
  }

  logger.system.info('session_ctrl: change matchmaking member max : ' + ' ' + matchmakingMenberNum + '=>'+ ' ' + membersMax);
  matchmakingMenberNum = membersMax;

  return res.send(200);
}

function handleError(res, err) {
  return res.send(500, err);
}
