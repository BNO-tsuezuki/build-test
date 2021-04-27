'use strict';

var _ = require('lodash');
var Gamestat = require('./gamestat.model');
var mongoose = require('mongoose');
var logger = require('../../logger');

// Get list of gamestats
exports.index = function(req, res) {
  logger.system.info('# gamestat:root');

  Gamestat.find(function (err, gamestats) {
    if(err) { return handleError(res, err); }
    return res.status(200).json(gamestats);
  }).lean();
};

// Get a single gamestat
exports.show = function(req, res) {
  logger.system.info('# gamestat:show');
  var id = new mongoose.Types.ObjectId(req.params.id);
  Gamestat.findById(id, function (err, gamestat) {
    if(err) { return handleError(res, err); }
    if(!gamestat) { return res.status(404).send('Not Found'); }
    return res.json(gamestat);
  });
};
exports.findLevel = function(req, res) {
  logger.system.info('# gamestat:find/level/' + req.params.id);
  Gamestat.find({level:req.params.id}, function (err, gamestat) {
    if(err) { return handleError(res, err); }
    if(!gamestat) { return res.status(404).send('Not Found'); }
    return res.json(200, gamestat);
  });
};


// Creates a new gamestat in the DB.
exports.create = function(req, res) {
  req.body.date = Date.now();
  req.body.created = Date.now();
  Gamestat.create(req.body, function(err, gamestat) {
    if(err) { return handleError(res, err); }

/*
    var i, j;
    logger.system.info('gamestat: create {');
    logger.system.info('  gamemode : ' + req.body['gamemode']);
    logger.system.info('  level    : ' + req.body['level']);
    logger.system.info('  winner   : ' + req.body['winner_team']);
    for( i = 0; i < req.body['timeline'].length; i++ )
    {
    logger.system.info('  timeline : [');
    logger.system.info('    time       : ' + req.body['timeline'][i]['time']);
    logger.system.info('    event_name : ' + req.body['timeline'][i]['event_name']);
    for( j = 0; j < req.body['timeline'][i]['params'].length; j++ )
    {
    logger.system.info('    params     : [');
    logger.system.info('      key   : ' + req.body['timeline'][i]['params'][j]['key']);
    logger.system.info('      value : ' + req.body['timeline'][i]['params'][j]['value']);
    logger.system.info('    ]');
    }
    logger.system.info('  ]');
    }
    logger.system.info('}');
*/

    return res.status(201).json(gamestat);
  });
};

// Updates an existing gamestat in the DB.
exports.update = function(req, res) {
  if(req.body._id) { delete req.body._id; }
  Gamestat.findById(req.params.id, function (err, gamestat) {
    if (err) { return handleError(res, err); }
    if(!gamestat) { return res.status(404).send('Not Found'); }
    var updated = _.merge(gamestat, req.body);
    updated.save(function (err) {
      if (err) { return handleError(res, err); }
      return res.status(200).json(gamestat);
    });
  });
};

// Deletes a gamestat from the DB.
exports.destroy = function(req, res) {
  var id = new mongoose.Types.ObjectId(req.params.id);
  Gamestat.findById(id, function (err, gamestat) {
    if(err) { return handleError(res, err); }
    if(!gamestat) { return res.status(404).send('Not Found'); }
    gamestat.remove(function(err) {
      if(err) { return handleError(res, err); }
      return res.status(204).send('No Content');
    });
  });
};

function handleError(res, err) {
  return res.status(500).send(err);
}
