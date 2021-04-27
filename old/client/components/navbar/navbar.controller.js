'use strict';

angular.module('evoserverApp')
  .controller('NavbarCtrl', function ($scope, $location, Auth) {
    $scope.menu = [{
      'title': 'Home',
      'link': '/'
    }, {
      'title': 'カスタマイズ',
      'link': '/customize',
      'loggedInOnly': true
    }, {
      'title': 'ショップ',
      'link': '/shop',
      'loggedInOnly': true
    }, {
      'title': 'GameStats',
      'link': '/gamestat',
      'loggedInOnly': false
    }, {
      'title': 'PerformanceStats',
      'link': '/performancestat',
      'loggedInOnly': false
    }, {
      'title': 'UserStats',
      'link': '/userstat',
      'loggedInOnly': false
    }, {
      'title': 'Sessions',
      'link': '/sessions',
      'loggedInOnly': false
    }, {
      'title': 'Matchmaking',
      'link': '/matchmaking',
      'loggedInOnly': false
    }
// 2016/03/07 機体の追加は機体ツリー経由で行うようになった
//    , {
//    'title': 'ショップ',
//    'link': '/shop',
//    'loggedInOnly': true
//  }
    ];

    $scope.isLoggedIn = Auth.isLoggedIn;
    $scope.isAdmin = Auth.isAdmin;
    $scope.getCurrentUser = Auth.getCurrentUser;

    $scope.logout = function() {
      Auth.logout();
      $location.path('/login');
    };

    $scope.isActive = function(route) {
      return route === $location.path();
    };

    $scope.menuFilter = function(item) {
      if (item.loggedInOnly) {
        return $scope.isLoggedIn();
      }
      return true;
    };
  });
