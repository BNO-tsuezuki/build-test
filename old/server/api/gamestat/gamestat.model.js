'use strict';

var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

var GamestatSchema = new Schema({
  date: Date,
  created: Date,
  gamemode: String,
  level: String,
  gametime: Number,
  winner_team: Number,
  initial_ticket: Number,
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
  ],
  teams:
  [
    {
      team: Number,
      remain_ticket: Number,
      decreased_ticket_by_killed: Number,
      decreased_ticket_by_conquered: Number,
      players:
      [
        {
          name: String,
          kill_count: Number,
          seriouslydamaged_count: Number,
          death_count: Number,
          recovery_count: Number,
          fob_conquering: Number,
          hq_attacking: Number
        }
      ]
    }
  ]
});

module.exports = mongoose.model('Gamestat', GamestatSchema);
