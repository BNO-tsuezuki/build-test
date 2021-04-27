'use strict';

angular.module('evoserverApp')
  .controller('ShopCtrl', function ($scope, $http, $uibModal, Master) {
    $scope.mss = [];

    Master.mobilesuitsAsArray()
    .then(function(mss) {
      $scope.mss = mss;
    });

    $scope.purchase = function(ms) {
      var modalInstance = $uibModal.open({
        animation: true,
        templateUrl: 'app/shop/shop.purchase.confirmation.modal.html',
        controller: 'ShopPurchaseConfirmationModalInstanceCtrl',
        size: 'lg',
        resolve: {
          ms: function () {
            return ms;
          }
        }
      });

      modalInstance.result.then(function (ms) {
        // 購入処理
        var modalInstance = $uibModal.open({
          animation: true,
          templateUrl: 'app/shop/shop.purchase.modal.html',
          controller: 'ShopPurchaseModalInstanceCtrl',
          size: 'lg',
          backdrop: 'static', // 閉じさせない
          resolve: {
            ms: function () {
              return ms;
            }
          }
        });

        modalInstance.result.then(function () {
        });

      }, function () {
        // キャンセル
      });
    };

  });

angular.module('evoserverApp')
  .controller('ShopPurchaseConfirmationModalInstanceCtrl', function ($scope, $uibModalInstance, ms) {

    $scope.ms = ms;

    $scope.ok = function () {
      $uibModalInstance.close($scope.ms);
    };

    $scope.cancel = function () {
      $uibModalInstance.dismiss('cancel');
    };

  });

angular.module('evoserverApp')
  .controller('ShopPurchaseModalInstanceCtrl', function ($scope, $uibModalInstance, Shop, ms, $timeout, $interval) {

    $scope.ms = ms;
    $scope.status = '購入処理中';
    $scope.message = 'しばらくお待ち下さい……';
    $scope.sequence = 'progress';
    $scope.progress = 10;

    Shop.purchase(ms.Name)
    .then(function (res) {
      $timeout(function () {
        $scope.progress = 100;
        $timeout(function () {
          $scope.status = '購入完了！';
          $scope.message = ms.displayName + 'の購入が完了しました！';
          $scope.sequence = 'completed';
        }, 500);
      }, 1000);
    }, function (err) {
      $scope.status = '購入失敗';
      $scope.message = 'エラー：' + err;
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
    }

  });
