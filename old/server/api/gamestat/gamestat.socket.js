/**
 * Broadcast updates to client when the model changes
 */

'use strict';

var Gamestat = require('./gamestat.model');

exports.register = function(socket) {
  Gamestat.schema.post('save', function (doc) {
    onSave(socket, doc);
  });
  Gamestat.schema.post('remove', function (doc) {
    onRemove(socket, doc);
  });
}

function onSave(socket, doc, cb) {
  socket.emit('gamestat:save', doc);
}

function onRemove(socket, doc, cb) {
  socket.emit('gamestat:remove', doc);
}