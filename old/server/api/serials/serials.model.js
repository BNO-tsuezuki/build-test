'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var SerialsSchema = new Schema({
  _id: { type: String, default: 'Serials' },
  serial_mobilesuit: { type: Number, min: 0, default: 0 },
  serial_gamestats: { type: Number, min: 0, default: 0 }
});

SerialsSchema.statics.getNewSerial = function (serialName, next) {
  var doc = {};
  doc[serialName] = 1;
  this.findOneAndUpdate(
    { _id : 'Serials' },
    { $inc : doc },
    { new : true },
    function (err, data) {
      if (err) {
        next( err, -1 );
      } else {
        next( null, data[serialName] );
      }
    }
  );
}

module.exports = mongoose.model('Serials', SerialsSchema);
