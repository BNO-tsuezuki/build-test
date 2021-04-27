'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('deck', {
        url: '/customize/deck',
        templateUrl: 'app/customize/deck/deck.html',
        controller: 'DeckCtrl'
      });
  });