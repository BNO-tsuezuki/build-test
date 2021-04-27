'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('gamestat_details', {
        url: '/gamestat_details',
        templateUrl: 'app/gamestat_details/gamestat_details.html',
        controller: 'GamestatDetailsCtrl'
      });
  });
