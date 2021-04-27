'use strict'

var _ = require('lodash');
var fs = require('fs');
var config = require('../environment');
var path = '/server/config/master/';

var load = function (name) {
  var arrayData = JSON.parse(fs.readFileSync(config.root + path + name + '.json'))[name];
  // DataTableのキー値は 'Name' で出力される
  var objectData = _.zipObject(_.pluck(arrayData, 'Name'), arrayData);
  return {
    array: arrayData,
    object: objectData,
    findById: function(id, cb) {
      var ms = objectData[id];
      if (!ms) {
        cb('invalid id: ' + id);
      } else {
        cb(null, ms);
      }
    }
  };
};

exports.getWeaponSlotOrderNum = function(name) {
  switch (name){
    case 'Primary':
      return 0;
      break;
    case 'Secondary':
      return 1;
      break;
    case 'Tertiary':
      return 2;
      break;
    default:
      return -1;
      break;
  }
};

exports.mobilesuits = load('mobilesuits');
exports.equipments = load('equipments');
exports.devtree = load('devtree');
exports.init_ms_lot = load('initialMobileSuitLot');
exports.resultReward = load('resultReward');
