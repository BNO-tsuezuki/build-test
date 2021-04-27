'use strict';

angular.module('evoserverApp')
  .controller('PerformancestatDetailsCtrl', function ($scope, $http) {
    $scope.message = 'Hello';

    // 親の$scopeを継承していることを確認
    //console.log($scope.performancestats);

    // table sorting override
    $scope.sortType = 'name';
    $scope.sortReverse = true;

    $scope.sortTable = function(sortType) {
      if ($scope.sortType === sortType) {
        $scope.sortReverse = !$scope.sortReverse;
      } else {
        $scope.sortType = sortType;
        $scope.sortReverse = true;
      }
    };

    $scope.delete = function(id) {
      $http.delete('api/performancestats/' + id).then(function(/*res*/) {
        // this callback will be called asynchronously
        // when the response is available
        $scope.parent.performancestats.splice($scope.parent.performancestats.indexOf($scope.performancestat), 1);
      }, function(res) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
        console.log(res);
      });
    };

  });
