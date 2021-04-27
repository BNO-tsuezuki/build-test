'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var MobileSuitStatsSchema = new Schema({
  userId: mongoose.Schema.Types.ObjectId,
  msId: { type: String, default: ''},
  win: { type: Number, min: 0, default: 0},
  lose: { type: Number, min: 0, default: 0},
  time: { type: Number, min: 0, default: 0},
  sortie: { type: Number, min: 0, default: 0},
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

  lifespan_average: { type: Number, min: 0, default: 0},
  lifespan_longest: { type: Number, min: 0, default: 0},
  skill_use: { type: Number, min: 0, default: 0},
  damage_guard: { type: Number, min: 0, default: 0},
  hit_rate: { type: Number, min: 0, default: 0},

  damage_source:
  [
    {
      name: { type: String, default: ''},
      kill: { type: Number, min: 0, default: 0},
      damage_given: { type: Number, min: 0, default: 0},
      reload: { type: Number, min: 0, default: 0},
      emit_count: { type: Number, min: 0, default: 0},
      hit_count: { type: Number, min: 0, default: 0},
      headshot_count: { type: Number, min: 0, default: 0},
      backshot_count: { type: Number, min: 0, default: 0}
    }
  ]
});

module.exports = mongoose.model('mobilesuitstat', MobileSuitStatsSchema);
