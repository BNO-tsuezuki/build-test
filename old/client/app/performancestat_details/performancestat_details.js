'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('performancestat_details', {
        url: '/performancestat_details',
        templateUrl: 'app/performancestat_details/performancestat_details.html',
        controller: 'PerformancestatDetailsCtrl'
      });
  });
