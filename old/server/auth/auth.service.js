'use strict';

var mongoose = require('mongoose');
var passport = require('passport');
var config = require('../config/environment');
var jwt = require('jsonwebtoken');
var expressJwt = require('express-jwt');
var compose = require('composable-middleware');
var User = require('../api/user/user.model');
var validateJwt = expressJwt({ secret: config.secrets.session });
var logger = require('../logger');

/*
var redisClient = require('redis').createClient({
  host: config.redis.session.host,
  port: config.redis.session.port
});
redisClient.select(config.redis.session.db);
redisClient.on('error', function (err) {
  logger.system.error('[redis] ' + err);
});
*/

/**
 * Attaches the user object to the request if authenticated
 * Otherwise returns 403
 */
function isAuthenticated() {
  return compose()
    // Validate jwt
    .use(function(req, res, next) {
      // allow access_token to be passed through query parameter as well
      if(req.query && req.query.hasOwnProperty('access_token')) {
        req.headers.authorization = 'Bearer ' + req.query.access_token;
      }
      validateJwt(req, res, next);
    })
    // Attach user to request
    .use(function(req, res, next) {
      User.findById(req.user._id, function (err, user) {
        if (err) return next(err);
        if (!user) return res.send(401);

        req.user = user;
        next();
      });
    });
}

/**
 * Checks if the user role meets the minimum requirements of the route
 */
function hasRole(roleRequired) {
  if (!roleRequired) throw new Error('Required role needs to be set');

  return compose()
    .use(isAuthenticated())
    .use(function meetsRequirements(req, res, next) {
      if (config.userRoles.indexOf(req.user.role) >= config.userRoles.indexOf(roleRequired)) {
        next();
      }
      else {
        res.send(403);
      }
    });
}

/**
 * Returns a jwt token signed by the app secret
 */
function signToken(id) {
  return jwt.sign({ _id: id }, config.secrets.session, { expiresInMinutes: 60*5 });
}

/**
 * Set token cookie directly for oAuth strategies
 */
function setTokenCookie(req, res) {
  if (!req.user) return res.json(404, { message: 'Something went wrong, please try again.'});
  var token = signToken(req.user._id, req.user.role);
  res.cookie('token', JSON.stringify(token));
  res.redirect('/');
}

/**
 * ゲームクライアントからのログインによるセッションを特別にストアしておく。
 * 既にストアされているセッションが存在した場合、そのセッションを破棄する。
 */
function storeGameClientSession(req, res) {
/*
  if (!req.user) return res.json(404, { message: 'Something went wrong, please try again.'});
  var userId = req.user.id;
  var hashkey = config.redis.session.gameclient_hashkey;
  redisClient.hget([hashkey, userId], function (err, reply) {

    // already exists, destroy old session
    if (reply) {
      logger.system.info('gameclient multiple login: user[' + req.user.name + ']');
      var oldSession = reply;
      redisClient.del(oldSession, function(err, reply) {
        if (err) {
          logger.system.error('[redis] ' + err);
        } else {
          logger.system.info('delete old session: ' + oldSession);
        }
      });
    }

    // store new session
    logger.system.info('login from gameclient: user[' + req.user.name + ']');
    var newSession = config.redis.session.prefix + req.sessionID;
    redisClient.hset([hashkey, userId, newSession], function (err, reply) {
      if (err) {
        logger.system.error('[redis] ' + err);
      }
    });

  });
*/
};

exports.isAuthenticated = isAuthenticated;
exports.hasRole = hasRole;
exports.signToken = signToken;
exports.setTokenCookie = setTokenCookie;
exports.storeGameClientSession = storeGameClientSession;
