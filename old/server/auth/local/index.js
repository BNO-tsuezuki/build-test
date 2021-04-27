'use strict';

var express = require('express');
var passport = require('passport');
var auth = require('../auth.service');
var logger = require('../../logger');

var router = express.Router();

router.post('/', function(req, res, next) {
  passport.authenticate('local', function (err, user, info) {
    var error = err || info;
    if (error) return res.json(401, error);
    if (!user) return res.json(404, {message: 'Something went wrong, please try again.'});

    logger.system.info('logged in [' + user.name + ']');

/*
    req.logIn(user, function(err) {
      if (err) {
        return res.json(500, err);
      }
    });

    // @todo どうにかしてゲームクライアントからのログインであることを知る
    auth.storeGameClientSession(req, res);
*/
    var token = auth.signToken(user._id, user.role);
//    res.json({token: token, sid: req.sessionID});
    res.json({token: token});
  })(req, res, next)
});

module.exports = router;