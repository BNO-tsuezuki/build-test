'use strict';

angular.module('evoserverApp')
  .factory('Customize', function Auth($http, User, UserMobilesuit, $cookieStore, $q) {
    var currentUser = {};
    if($cookieStore.get('token')) {
      currentUser = User.get();
    }

    return {

      /**
       * Update user deck
       *
       * @param  {[ObjectId]} newDeck
       * @param  {Function}   callback    - optional
       * @return {Promise}
       */
      updateDeck: function(newDeck, callback) {
        var cb = callback || angular.noop;

        return User.updateDeck({ id: currentUser._id }, {
          newDeck: newDeck
        }, function(user) {
          return cb(user);
        }, function(err) {
          return cb(err);
        }).$promise;
      },

      /**
       * Update ms inventory
       *
       * @param  ObjectId usermobilesuit id
       * @param  [String] newInventory
       * @param  String newColor
       * @param  String newBuffs
       * @param  {Function}   callback    - optional
       * @return {Promise}
       */
      updateInventory: function(msId, newInventory, newColor, newBuffs, callback) {
        var cb = callback || angular.noop;

        return UserMobilesuit.updateInventory({ id: msId }, {
          newInventory: newInventory,
          newColor: newColor,
          newBuffs: newBuffs
        }, function() {
          return cb();
        }, function(err) {
          return cb(err);
        }).$promise;
      }
    };

  });
