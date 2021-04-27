/**
 * Broadcast updates to client when the model changes
 */

'use strict';

var Performancestat = require('./performancestat.model');

exports.register = function(socket) {
  Performancestat.schema.post('save', function (doc) {
    onSave(socket, doc);
  });
  Performancestat.schema.post('remove', function (doc) {
    onRemove(socket, doc);
  });
}

function onSave(socket, doc, cb) {
  socket.emit('performancestat:save', doc);
}

function onRemove(socket, doc, cb) {
  socket.emit('performancestat:remove', doc);
}
