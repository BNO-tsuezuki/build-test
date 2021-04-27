'use strict';

var _ = require('lodash');
var Gamestat = require('./performancestat.model');
var mongoose = require('mongoose');
var logger = require('../../logger');

// Get list of performancestats
exports.index = function(req, res) {
  Gamestat.find(function (err, performancestats) {
    if(err) { return handleError(res, err); }
    return res.status(200).json(performancestats);
  }).lean();
};

// Get a single performancestat
exports.show = function(req, res) {
  var id = new mongoose.Types.ObjectId(req.params.id);
  Gamestat.findById(id, function (err, performancestat) {
    if(err) { return handleError(res, err); }
    if(!performancestat) { return res.status(404).send('Not Found'); }
    return res.json(performancestat);
  });
};

// Creates a new performancestat in the DB.
exports.create = function(req, res) {
  req.body.date = Date.now();
  req.body.created = Date.now();

  var num = parseInt(req.body.timeline.length);
  if (num === 0) {
      return res.status(204).send('No Content');
  }

  var fps = {};
  var game = {};
  var render = {};
  var gpu = {};

  var setParam = function(data, param) {
    var params = param.value.split(' ');
    if (!('total' in data)) { data.total = 0; };
    if (!('max' in data)) { data.max = params[1]; };
    if (!('min' in data)) { data.min = data.max; };
    data.total += parseFloat(params[0], 10);
    data.max = Math.max(data.max, parseFloat(params[1], 10));
    data.min = Math.min(data.min, parseFloat(params[2], 10));
  };
  _.each(req.body.timeline, function(timeline, index, array) {
    setParam(fps, _.find(timeline.params, function(param) { return param.key === 'fps'; }));
    setParam(game, _.find(timeline.params, function(param) { return param.key === 'game'; }));
    setParam(render, _.find(timeline.params, function(param) { return param.key === 'render'; }));
    setParam(gpu, _.find(timeline.params, function(param) { return param.key === 'gpu'; }));
  });

  var setBody = function(body, key, data) {
    body[key] = (data.total / num);
    body[key + '_max'] = data.max;
    body[key + '_min'] = data.min;
  }
  setBody(req.body, 'fps', fps);
  setBody(req.body, 'game', game);
  setBody(req.body, 'render', render);
  setBody(req.body, 'gpu', gpu);

  Gamestat.create(req.body, function(err, performancestat) {
    if(err) { return handleError(res, err); }
    return res.status(201).json(performancestat);
  });
};

// Updates an existing performancestat in the DB.
exports.update = function(req, res) {
  if(req.body._id) { delete req.body._id; }
  Gamestat.findById(req.params.id, function (err, performancestat) {
    if (err) { return handleError(res, err); }
    if(!performancestat) { return res.status(404).send('Not Found'); }
    var updated = _.merge(performancestat, req.body);
    updated.save(function (err) {
      if (err) { return handleError(res, err); }
      return res.status(200).json(performancestat);
    });
  });
};

// Deletes a performancestat from the DB.
exports.destroy = function(req, res) {
  var id = new mongoose.Types.ObjectId(req.params.id);
  Gamestat.findById(id, function (err, performancestat) {
    if(err) { return handleError(res, err); }
    if(!performancestat) { return res.status(404).send('Not Found'); }
    performancestat.remove(function(err) {
      if(err) { return handleError(res, err); }
      return res.status(204).send('No Content');
    });
  });
};

function handleError(res, err) {
  return res.status(500).send(err);
}
