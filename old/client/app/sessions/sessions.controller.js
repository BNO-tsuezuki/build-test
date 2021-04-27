'use strict';

angular.module('evoserverApp')
  .controller('SessionCtrl', function ($scope, $http, $interval) {

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

    $scope.getData = function() {
      $http.get('api/sessions/').then(function(res) {
        // this callback will be called asynchronously
        // when the response is available
        $scope.sessions = [];
        var i = 0;
        Object.keys(res.data).forEach(function(key){
          if (key != 'hosts') {
            $scope.sessions[i] = res.data[key];
            i++;
          }
        });
      }, function(res) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
        console.log(res);
      });
    };

    $scope.$on("$destroy", function() {
      $interval.cancel($scope.updateTimer);
    });
    $scope.updateTimer = $interval($scope.getData, 3000);
    $scope.getData();
  });
