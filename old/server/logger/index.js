'use strict'

var log4js = require('log4js');
var config = require('../config/environment');

log4js.configure(config.log4js);

module.exports = {
  access: log4js.getLogger('access'),
  system: log4js.getLogger('system'),
  error: log4js.getLogger('error'),
  express: log4js.connectLogger(log4js.getLogger('access'), {
    level: log4js.levels.INFO
  })
};
