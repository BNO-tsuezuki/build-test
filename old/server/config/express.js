/**
 * Express configuration
 */

'use strict';

var express = require('express');
//var session = require('express-session');
//var RedisStore = require('connect-redis')(session);
var favicon = require('static-favicon');
var logger = require('../logger');
var compression = require('compression');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var cookieParser = require('cookie-parser');
var errorHandler = require('errorhandler');
var path = require('path');
var config = require('./environment');
var passport = require('passport');

module.exports = function(app) {
  var env = app.get('env');

  app.set('views', config.root + '/server/views');
  app.engine('html', require('ejs').renderFile);
  app.set('view engine', 'html');
  app.use(compression());
  app.use(bodyParser.urlencoded({ limit: '1mb', extended: false }));
  app.use(bodyParser.json({limit: '1mb'}));
  app.use(methodOverride());
  app.use(cookieParser());
/*
  app.use(session({
    key: 'evo.sid',
    store: new RedisStore({
      host: config.redis.session.host,
      port: config.redis.session.port,
      db: config.redis.session.db,
      //prefix: config.redis.session.prefix,
      ttl: 60 * 5
    }),
    secret: config.secrets.session
  }));
*/
  app.use(passport.initialize());
//  app.use(passport.session());

  if ('production' === env) {
    app.use(favicon(path.join(config.root, 'public', 'favicon.ico')));
    app.use(express.static(path.join(config.root, 'public')));
    app.set('appPath', config.root + '/public');
    app.use(logger.express);
  }

  if ('development' === env || 'test' === env) {
    app.use(require('connect-livereload')());
    app.use(express.static(path.join(config.root, '.tmp')));
    app.use(express.static(path.join(config.root, 'client')));
    app.set('appPath', 'client');
    app.use(logger.express);
    app.use(errorHandler()); // Error handler - has to be last
  }
};
