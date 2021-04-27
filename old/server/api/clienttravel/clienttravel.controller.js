'use strict';

var _ = require('lodash');
var logger = require('../../logger');

exports.clientTravel = function(req, res) {
  if (!req.body.from || !req.body.to) {
    return handleError(res, 'invalid travel');
  }
  var from = req.body.from;
  var to = req.body.to;
  logger.system.info('client travel %s -> %s', from, to);
  return res.json(200, {});
};

function handleError(res, err) {
  return res.send(500, err);
}
