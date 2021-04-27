'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var PerformancestatSchema = new Schema({
  date: Date,
  created: Date,
  gamemode: String,
  level: String,
  gametime: Number,
  userid: String,
  playername: String,
  fps: Number,
  fps_max: Number,
  fps_min: Number,
  game: Number,
  game_max: Number,
  game_min: Number,
  render: Number,
  render_max: Number,
  render_min: Number,
  gpu: Number,
  gpu_max: Number,
  gpu_min: Number,
  timeline:
  [
    {
      time: Number,
      event_name: String,
      params:
      [
        {
          key: String,
          value: String
        }
      ]
    }
  ]
});

module.exports = mongoose.model('Performancestat', PerformancestatSchema);
