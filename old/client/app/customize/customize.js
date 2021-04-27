'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('customize', {
        url: '/customize',
        templateUrl: 'app/customize/customize.html',
        controller: 'CustomizeCtrl'
      });
  });
