'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('sessions', {
        url: '/sessions',
        templateUrl: 'app/sessions/sessions.html',
        controller: 'SessionCtrl'
      });
  });
