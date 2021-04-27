'use strict';

var _ = require('lodash');

// Get list of matchings
exports.index = function(req, res) {
  Matching.find(function (err, matchings) {
    if(err) { return handleError(res, err); }
    return res.json(200, matchings);
  });
};

// Get a single matching
exports.show = function(req, res) {
  Matching.findById(req.params.id, function (err, matching) {
    if(err) { return handleError(res, err); }
    if(!matching) { return res.send(404); }
    return res.json(matching);
  });
};

// Creates a new matching in the DB.
exports.create = function(req, res) {
  Matching.create(req.body, function(err, matching) {
    if(err) { return handleError(res, err); }
    return res.json(201, matching);
  });
};

// Updates an existing matching in the DB.
exports.update = function(req, res) {
  if(req.body._id) { delete req.body._id; }
  Matching.findById(req.params.id, function (err, matching) {
    if (err) { return handleError(res, err); }
    if(!matching) { return res.send(404); }
    var updated = _.merge(matching, req.body);
    updated.save(function (err) {
      if (err) { return handleError(res, err); }
      return res.json(200, matching);
    });
  });
};

// Deletes a matching from the DB.
exports.destroy = function(req, res) {
  Matching.findById(req.params.id, function (err, matching) {
    if(err) { return handleError(res, err); }
    if(!matching) { return res.send(404); }
    matching.remove(function(err) {
      if(err) { return handleError(res, err); }
      return res.send(204);
    });
  });
};

function handleError(res, err) {
  return res.send(500, err);
}
