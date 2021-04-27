'use strict';

var path = require('path');
var _ = require('lodash');

function requiredProcessEnv(name) {
  if(!process.env[name]) {
    throw new Error('You must set the ' + name + ' environment variable');
  }
  return process.env[name];
}

// All configurations will extend these options
// ============================================
var all = {
  env: process.env.NODE_ENV,

  // Root path of server
  root: path.normalize(__dirname + '/../../..'),

  // Server port
  port: process.env.PORT || 9000,

  // Should we populate the DB with sample data?
  seedDB: false,

  branch: 'master',

  // Secret for session, you will want to change this and make it an environment variable
  secrets: {
    session: 'evoserver-secret'
  },

  // List of user roles
  userRoles: ['guest', 'user', 'admin'],

  // MongoDB connection options
  mongo: {
    options: {
      db: {
        safe: true
      }
    }
  },

  redis: {
    session: {
      host: 'localhost',
      port: 6379,
      db: 1,
      prefix: 'sess:',
      gameclient_hashkey: 'gcsess'
    }
  },

  // logger configuration
  log4js: {
    appenders: [{
      category: 'access',
      type: 'dateFile',
      filename: '/tmp/access.log',
      pattern: '-yyyy-MM-dd',
      backups: 3
    }, {
      category: 'system',
      type: 'dateFile',
      filename: '/tmp/system.log',
      pattern: '-yyyy-MM-dd',
      backups: 3
    }, {
      category: 'error',
      type: 'dateFile',
      filename: '/tmp/error.log',
      pattern: '-yyyy-MM-dd',
      backups: 3
    }, {
      type: 'console'
    }
    ],
    levels: {
      access: 'ALL',
      system: 'ALL',
      error: 'ALL'
    }
  }

};

// Export the config object based on the NODE_ENV
// ==============================================
module.exports = _.merge(
  all,
  require('./' + process.env.NODE_ENV + '.js') || {});
