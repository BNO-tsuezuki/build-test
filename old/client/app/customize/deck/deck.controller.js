'use strict';

angular.module('evoserverApp')
  .controller('DeckCtrl', function ($scope, $location, $http, Customize, lodash, Master) {
    $scope.mobilesuits = [];
    $scope.availableMobilesuits1 = [];
    $scope.availableMobilesuits2 = [];
    $scope.availableMobilesuits3 = [];
    $scope.availableMobilesuits4 = [];

    $scope.back = function () {
      $location.path('customize');
    }

    Master.mobilesuitsAsObject()
    .then(function(mss) {
      $scope.mss = mss;
      $http.get('/api/user/mobilesuits')
      .success(function(mobilesuits) {
        $scope.mobilesuits = mobilesuits;
        $http.get('/api/users/me').success(function(user) {
          $scope.ms1 = user.deck[0];
          $scope.ms2 = user.deck[1];
          $scope.ms3 = user.deck[2];
          $scope.ms4 = user.deck[3];
          $scope.onChange();
        });
      });
    });

    $scope.showAlert = false;
    $scope.alertType = 'success';

    $scope.updateDeck = function() {
      Customize.updateDeck([$scope.ms1])
        .then(function() {
          $scope.showAlert = true;
          $scope.alertType = 'success';
          $scope.message = 'デッキを更新しました。';
        })
        .catch(function(err) {
          $scope.showAlert = true;
          $scope.alertType = 'danger';
          $scope.message = 'デッキの更新に失敗しました。 : ' + err.data;
        });
    };

    $scope.closeAlert = function() {
      $scope.showAlert = false;
      $scope.message = '';
    };

    $scope.onChange = function () {
      var unselected = {
        _id: '',
        DisplayName: '未選択'
      };
      $scope.availableMobilesuits1 = lodash.cloneDeep($scope.mobilesuits);
      $scope.availableMobilesuits2 = lodash.cloneDeep($scope.mobilesuits);
      $scope.availableMobilesuits3 = lodash.cloneDeep($scope.mobilesuits);
      $scope.availableMobilesuits4 = lodash.cloneDeep($scope.mobilesuits);

      lodash.remove($scope.availableMobilesuits1, function(ms) {
        return ms._id === $scope.ms2 || ms._id === $scope.ms3 || ms._id === $scope.ms4;
      });
      lodash.remove($scope.availableMobilesuits2, function(ms) {
        return ms._id === $scope.ms1 || ms._id === $scope.ms3 || ms._id === $scope.ms4;
      });
      lodash.remove($scope.availableMobilesuits3, function(ms) {
        return ms._id === $scope.ms1 || ms._id === $scope.ms2 || ms._id === $scope.ms4;
      });
      lodash.remove($scope.availableMobilesuits4, function(ms) {
        return ms._id === $scope.ms1 || ms._id === $scope.ms2 || ms._id === $scope.ms3;
      });

      $scope.availableMobilesuits1.unshift(unselected);
      $scope.availableMobilesuits2.unshift(unselected);
      $scope.availableMobilesuits3.unshift(unselected);
      $scope.availableMobilesuits4.unshift(unselected);
    };

  });
