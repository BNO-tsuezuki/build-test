'use strict';

var _ = require('lodash');
var ms = require('../user/mobilesuit/mobilesuit.controller');
var logger = require('../../logger');

exports.purchase = function(req, res) {
  if (!req.body.id) { 
    return handleError(res, 'invalid id: ' + req.body.id);
  }
  
  logger.system.info('user [' + req.user.name + '] purchased item [' + req.body.id + ']');
  return ms.create(req, res);
};

function handleError(res, err) {
  return res.send(500, err);
}