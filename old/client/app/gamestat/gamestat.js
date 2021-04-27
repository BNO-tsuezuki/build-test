'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('gamestat', {
        url: '/gamestat',
        templateUrl: 'app/gamestat/gamestat.html',
        controller: 'GamestatCtrl'
      });
  });
