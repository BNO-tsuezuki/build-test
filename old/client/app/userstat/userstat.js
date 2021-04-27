'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('userstat', {
        url: '/userstat',
        templateUrl: 'app/userstat/userstat.html',
        controller: 'UserstatCtrl'
      });
  });
