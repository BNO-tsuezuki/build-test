'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('mobilesuit', {
        url: '/customize/mobilesuit',
        templateUrl: 'app/customize/mobilesuit/mobilesuit.html',
        controller: 'MobilesuitCtrl'
      });
  });