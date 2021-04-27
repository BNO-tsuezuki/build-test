'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var UserMobilesuitSchema = new Schema({
  _id: { type: Number, min:0, default: 0 },
  userId: mongoose.Schema.Types.ObjectId,
  msId: String,
  DisplayName: String,
  inventory: [ String ],
  color: {type: String, default: ''},
  buffs: {type: String, default: ''},
  exp: { type: Number, min: 0, default: 0 },
  unlockedWeapons: [ String ],
  unlockedColors: [ String ],
  unlockedBuffs: [ String ]
});

module.exports = mongoose.model('UserMobilesuit', UserMobilesuitSchema);
