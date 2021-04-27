'use strict';

angular.module('evoserverApp')
  .controller('MobilesuitCtrl', function ($scope, $stateParams, $location, Master, User, $interval, Customize, lodash) {
    $scope.back = function() {
      $location.path('customize');
    };

    $scope.ms = $stateParams.ms;
    if (!$scope.ms) {
      $scope.back();
      return;
    }

    $scope.primary = $scope.ms.ms.inventory[0] || '';
    $scope.secondary = $scope.ms.ms.inventory[1] || '';
    $scope.tertiary = $scope.ms.ms.inventory[2] || '';

    $scope.availablesPrimary = [];
    $scope.availablesSecondary = [];
    $scope.availablesTertiary = [];
    $scope.availablesColor = [];
    $scope.availablesBuff = [];

    $scope.color = { 'Name': '' };
    $scope.buffs = { 'Name': '' };

    Master.devtreeAsObject()
    .then(function (data) {
      $scope.devtree = data;
      {
        var availables = $scope.ms.ms.unlockedColors;
        for (var i = 0; i < availables.length; i++) {
          var available = availables[i];
          if (!available) { 
            continue;
          }
          var color = $scope.devtree[available];
          if (!color) {
            continue; 
          }
          $scope.availablesColor.push(color);
        }
      }
      {
        var availables = $scope.ms.ms.unlockedBuffs;
        for (var i = 0; i < availables.length; i++) {
          var available = availables[i];
          if (!available) { continue; }
          var buff = $scope.devtree[available];
          if (!buff) { continue; }
          $scope.availablesBuff.push(buff);
        }
      }

      Master.equipmentsAsObject()
      .then(function (data) {
        $scope.equipments = data;
        var availables = $scope.ms.ms.unlockedWeapons;
        for (var i = 0; i < availables.length; i++) {
          var available = availables[i];
          if (!available) { continue; }
          // DevtreeからEquipmentsに変換
          var devnode = $scope.devtree[available];
          if (!devnode) { continue; }
          var eqid = devnode.id || '';
          var eq = $scope.equipments[eqid];
          if (!eq) { continue; }
          if (eq.bIsAvailble) {
            if (eq.WeaponSlotOrder === 0) {
              $scope.availablesPrimary.push(eq);
            }
            else if (eq.WeaponSlotOrder === 1) {
              $scope.availablesSecondary.push(eq);
            }
            else if (eq.WeaponSlotOrder === 2) {
              $scope.availablesTertiary.push(eq);
            }
          }
        }
      });
    });

    $scope.showAlert = false;
    $scope.alertType = 'success';

    $scope.update = function () {
      Customize.updateInventory(
        $scope.ms.ms._id,
        [$scope.primary, $scope.secondary, $scope.tertiary],
        $scope.color.Name,
        $scope.buffs.Name
      )
      .then(function () {
        $scope.showAlert = true;
        $scope.alertType = 'success';
        $scope.message = '更新しました。';
      })
      .catch(function(err) {
        $scope.showAlert = true;
        $scope.alertType = 'danger';
        $scope.message = '更新に失敗しました。 : ' + err.data;
      });
    };

    $scope.closeAlert = function() {
      $scope.showAlert = false;
      $scope.message = '';
    };

  });
