'use strict';

angular.module('evoserverApp')
  .config(function ($stateProvider) {
    $stateProvider
      .state('shop', {
        url: '/shop',
        templateUrl: 'app/shop/shop.html',
        controller: 'ShopCtrl'
      });
  });
