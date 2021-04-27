'use strict';

var _ = require('lodash');
var logger = require('../../logger');

var serverList = {};

exports.registerServer = function(req, res) {
  var session = req.body.session;
  var host = req.body.host;
  var port = req.body.port;
  var addr = host + ":" + port;
  var lastAlive = Date.now();

  if (serverList[addr]) {
    return handleError(res, "server " + addr + " is already registered.");
  }

  serverList[addr] = {
    session: session,
    host: host,
    port: port,
    lastAlive: Date.now()
  };

  logger.system.info("register server: " + addr);

  return res.json(200);
};

exports.unregisterServer = function(req, res) {
  var host = req.body.host;
  var port = req.body.port;
  var addr = host + ":" + port;

  if (!serverList[addr]) {
    return handleError(res, "server " + addr + " is not exist.");
  }

  delete serverList[addr];
  logger.system.info("unregister server: " + addr);

  return res.json(200);
};

exports.keepalive = function(req, res) {
  var session = req.body.session;
  var host = req.body.host;
  var port = req.body.port;
  var addr = host + ":" + port;

  if (!serverList[addr]) {
    return handleError(res, "server " + addr + " is not exist.");
  }

  serverList[addr].session = session;
  serverList[addr].lastAlive = Date.now();

  return res.json(200);
};

exports.index = function(req, res) {
  var newList = {};
  _.each(serverList, function(server, addr, serverList) {
    if (Date.now() < (server.lastAlive + (30 * 1000))) {
      newList[addr] = server;
    }
  });
  serverList = newList;
  serverList.hosts = _.keys(newList);
  return res.json(200, serverList);
};

function handleError(res, err) {
  return res.send(500, err);
}
