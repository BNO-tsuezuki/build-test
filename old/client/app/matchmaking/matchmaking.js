'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('matchmaking', {
        url: '/matchmaking',
        templateUrl: 'app/matchmaking/matchmaking.html',
        controller: 'MatchmakingCtrl'
      });
  });
