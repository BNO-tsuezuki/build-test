/**
 * Populate DB with sample data on server start
 * to disable, edit config/environment/index.js, and set `seedDB: false`
 */

'use strict';

// for MongoDB
var Thing = require('../api/thing/thing.model');
var User = require('../api/user/user.model');
var UserMobilesuit = require('../api/user/mobilesuit/mobilesuit.model');
var Serials = require('../api/serials/serials.model');
var logger = require('../logger');
var _ = require('lodash');

Thing.find({}).remove(function() {
  Thing.create({
    name : 'Development Tools',
    info : 'Integration with popular tools such as Bower, Grunt, Karma, Mocha, JSHint, Node Inspector, Livereload, Protractor, Jade, Stylus, Sass, CoffeeScript, and Less.'
  }, {
    name : 'Server and Client integration',
    info : 'Built with a powerful and fun stack: MongoDB, Express, AngularJS, and Node.'
  }, {
    name : 'Smart Build System',
    info : 'Build system ignores `spec` files, allowing you to keep tests alongside code. Automatic injection of scripts and styles into your index.html'
  },  {
    name : 'Modular Structure',
    info : 'Best practice client and server structures allow for more code reusability and maximum scalability'
  },  {
    name : 'Optimized Build',
    info : 'Build process packs up your templates as a single JavaScript payload, minifies your scripts/css/images, and rewrites asset names for caching.'
  },{
    name : 'Deployment Ready',
    info : 'Easily deploy your app to Heroku or Openshift with the heroku and openshift subgenerators'
  });
});

UserMobilesuit.find({}).remove(function () {});

User.find({}).remove(function() {
  var msNum = 0;
  User.create({
    provider: 'local',
    name: 'Test User',
    email: 'test@test.com',
    password: 'test',
    gamePoint: 10000
  }, {
    provider: 'local',
    name: 'evoserver',
    email: 'evoserver@evoserver.evo',
    password: 'evoserver'
  }, {
    provider: 'local',
    name: 'yoda',
    email: 'yoda@test.com',
    password: 'test'
  }, {
    provider: 'local',
    role: 'admin',
    name: 'Admin',
    email: 'admin@admin.com',
    password: 'admin'
  }, function() {
      User.find({}, function (err, users) {
        _.forEach(users, function (user) {
/*
          // 初期デッキ用に４機体購入済みにしておく
          UserMobilesuit.create({
            _id: msNum+=1,
            userId: user._id,
            msId: 'MS0005',
            exp: 10000
          }, {
            _id: msNum+=1,
            userId: user._id,
            exp: 10000,
            msId: 'MS0006'
          }, {
            _id: msNum+=1,
            userId: user._id,
            exp: 10000,
            msId: 'MS0002'
          }, {
            _id: msNum+=1,
            userId: user._id,
            exp: 10000,
            msId: 'MS0114'
          }, function (err, gm, zk, gc, uc) {
            user.deck = [gm._id, zk._id, gc._id, uc._id];
            user.save(function (err) {
              Serials.find({}).remove(function () {
                Serials.create({
                  _id: 'Serials',
                  serial_mobilesuit: msNum
                  }, function (err) {}
                );
              });
            });
          });
*/
        });
      });

      logger.system.trace('finished populating users');
    }
  );
});

