'use strict';

angular.module('evoserverApp')
  .controller('CustomizeCtrl', function ($scope, $location, $http, Master, $uibModal, lodash) {

    $scope.masterMobilesuits = {};
    $scope.masterEquipments = {};
    $scope.mobilesuits = [];

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

    var getUserMobilesuits = function (master) {
      $scope.mobilesuits = [];
      $http.get('/api/user/mobilesuits')
      .success(function (mobilesuits) {
        lodash.forEach(mobilesuits, function(ms) {
          $scope.mobilesuits.push({
            master: $scope.masterMobilesuits[ms.msId],
            ms: ms
          });
        });
      });
    };

    Master.mobilesuitsAsObject()
    .then(function (master) {
      $scope.masterMobilesuits = master;
      Master.equipmentsAsObject()
      .then(function (master) {
        $scope.masterEquipments = master;
        getUserMobilesuits();
      });
    });

    $scope.findEquipment = function (eq) {
      return $scope.masterEquipments[eq] || { DisplayName: '未装備' };
    };

    // シーン遷移時に、遷移先に選んだMSを渡す
    $scope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      toParams.ms = $scope.selectedMs;
    });

    // MS個別カスタマイズへの遷移
    $scope.customizeMs = function(ms) {
      $scope.selectedMs = ms;
      $location.path('customize/mobilesuit');
    }

    // デッキカスタマイズ
    $scope.customizeDeck = function () {
      $location.path('customize/deck');
    }

    // MSの廃棄ボタンクリック
    $scope.disposeMs = function(ms) {
      var modalInstance = $uibModal.open({
        animation: true,
        templateUrl: 'app/customize/customize.disposal.confirmation.modal.html',
        controller: 'CustomizeDisposalConfirmationModalInstanceCtrl',
        size: 'lg',
        resolve: {
          ms: function () {
            return ms;
          }
        }
      });

      modalInstance.result.then(function (ms) {
        // MSの廃棄
        var modalInstance = $uibModal.open({
          animation: true,
          templateUrl: 'app/customize/customize.disposal.modal.html',
          controller: 'CustomizeDisposalModalInstanceCtrl',
          size: 'lg',
          backdrop: 'static', // 閉じさせない
          resolve: {
            ms: function () {
              return ms;
            }
          }
        });

        modalInstance.result.then(function () {
          getUserMobilesuits();
        });

      }, function () {
        // キャンセル
      });
    };

  });

angular.module('evoserverApp')
  .controller('CustomizeDisposalConfirmationModalInstanceCtrl', function ($scope, $uibModalInstance, ms) {

    $scope.ms = ms;

    $scope.ok = function () {
      $uibModalInstance.close($scope.ms);
    };

    $scope.cancel = function () {
      $uibModalInstance.dismiss('cancel');
    };

  });

angular.module('evoserverApp')
  .controller('CustomizeDisposalModalInstanceCtrl', function ($scope, $uibModalInstance, UserMobilesuit, ms, $timeout, $interval) {

    $scope.ms = ms;
    $scope.status = 'MSを廃棄中';
    $scope.message = 'しばらくお待ち下さい……';
    $scope.sequence = 'progress';
    $scope.progress = 10;

    UserMobilesuit.disposal({ id: ms.ms._id }, function () {
      $timeout(function () {
        $scope.progress = 100;
        $timeout(function () {
          $scope.status = '廃棄完了';
          $scope.message = ms.master.displayName + 'を廃棄しました。';
          $scope.sequence = 'completed';
        }, 500);
      }, 1000);
    }, function (err) {
      $scope.status = '廃棄失敗';
      $scope.message = '廃棄に失敗しました：' + err.data;
      $scope.sequence = 'failed';
    });

    $interval(function () {
      $scope.progress += 3;
    }, 100);

    $scope.ok = function () {
      $uibModalInstance.close($scope.ms);
    };

    $scope.cancel = function () {
      $uibModalInstance.dismiss('cancel');
    };

    $scope.inProgress = function () {
      return $scope.sequence === 'progress';
    };

  });
