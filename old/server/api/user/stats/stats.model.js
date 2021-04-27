'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var UserStatsSchema = new Schema({
  userId: mongoose.Schema.Types.ObjectId,
  win: { type: Number, min: 0, default: 0},
  lose: { type: Number, min: 0, default: 0},
  time: { type: Number, min: 0, default: 0},
  finalblows: { type: Number, min: 0, default: 0},
  eliminations: { type: Number, min: 0, default: 0},
  death: { type: Number, min: 0, default: 0},
  killstreak: { type: Number, min: 0, default: 0},
  damage_given: { type: Number, min: 0, default: 0},
  damage_taken: { type: Number, min: 0, default: 0},
  recovery: { type: Number, min: 0, default: 0},
  spotted: { type: Number, min: 0, default: 0},
  conquered_seconds: { type: Number, min: 0, default: 0},
  conquered_times: { type: Number, min: 0, default: 0},
  maintain_seconds: { type: Number, min: 0, default: 0},
  name: String,
  mobilesuit_stat: { type: Array, default: [] }
});

module.exports = mongoose.model('userstat', UserStatsSchema);
