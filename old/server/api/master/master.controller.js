'use strict';

var _ = require('lodash');
var Master = require('../../config/master');

// Get list of master/mobilesuits
exports.index = function(req, res) {
  var target = req.params.target;
  if (!target) { return res.send(404); }
  if (!Master[target]) { return res.send(404); }

  if (req.query.type === 'object') {
    return res.json(200, Master[target].object);
  }
  return res.json(200, Master[target].array);
};

// Get a single master/mobilesuit
exports.show = function(req, res) {
  var target = req.params.target;
  if (!target) { return res.send(404); }
  if (!Master[target]) { return res.send(404); }

  Master[target].findById(req.params.id, function(err, ms) {
    if (err) { return handleError(res, err); }
    if (!ms) { return res.send(404); }
    return res.json(ms);
  });
};

function handleError(res, err) {
  return res.send(500, err);
}