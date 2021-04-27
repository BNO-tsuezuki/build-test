'use strict';

angular.module('evoserverApp')
  .controller('PerformancestatCtrl', function ($scope, $http) {
    // use datepicker
    $scope.date = new Date();
    $scope.datePickerOpen = false;
    $scope.toggleDatePicker = function($event) {
      // これが重要らしい。
      $event.stopPropagation();
      $scope.datePickerOpen = !$scope.datePickerOpen;
    };

    // table sorting
    $scope.sortType = 'date';
    $scope.sortReverse = true;
    $scope.searchFish = '';

    $scope.sortTable = function(sortType) {
      if ($scope.sortType === sortType) {
        $scope.sortReverse = !$scope.sortReverse;
      } else {
        $scope.sortType = sortType;
        $scope.sortReverse = true;
      }
    };

    // データ取得
    $http.get('api/performancestats').then(function(res) {
      // this callback will be called asynchronously
      // when the response is available
      $scope.performancestats = res.data;
    }, function(res) {
      // called asynchronously if an error occurs
      // or server returns response with an error status.
      console.log(res);
    });
  });
