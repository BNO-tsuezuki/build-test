'use strict';

angular.module('evoserverApp')
  .factory('Session', function ($http, User, $cookieStore, $q) {
    var currentUser = {};
    if($cookieStore.get('token')) {
      currentUser = User.get();
    }

    return {

      forceMatchmaking: function(session, mapname, gamemode, demoRecordType, startupTime, restartNum, updateStats, users, callback) {
        var cb = callback || angular.noop;
        var deferred = $q.defer();

        $http.post('/api/sessions/matchmaking/force', {
          session: session,
          users: users,
          mapname: mapname,
          gamemode: gamemode,
          demorecordtype: demoRecordType,
          startuptime: startupTime,
          restartnum: restartNum,
          updatestats: updateStats,
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
