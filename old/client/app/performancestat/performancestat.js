'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('performancestat', {
        url: '/performancestat',
        templateUrl: 'app/performancestat/performancestat.html',
        controller: 'PerformancestatCtrl'
      });
  });
