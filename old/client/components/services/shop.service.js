'use strict';

angular.module('evoserverApp')
  .factory('Shop', function ($rootScope, $http, User, $cookieStore, $q) {
    var currentUser = {};
    if($cookieStore.get('token')) {
      currentUser = User.get();
    }

    return {

      /** 購入リクエスト。いまんとこMSだけになってるので後で直す */
      purchase: function(msid, callback) {
        var cb = callback || angular.noop;
        var deferred = $q.defer();

        $http.post('/api/shop/purchase', {
          id: msid
        })
        .success(function(data) {
          deferred.resolve(data);
          return cb();
        })
        .error(function(err) {
          deferred.reject(err);
          return cb(err);
        }.bind(this));

        return deferred.promise;
      }

    };
  });
