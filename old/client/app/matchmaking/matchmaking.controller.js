'use strict';

angular.module('evoserverApp')
  .controller('MatchmakingCtrl', function ($scope, $http, $interval, Session) {

    // Session table sorting
    $scope.sortSessionType = 'keepalive';
    $scope.sortSessionReverse = true;
    $scope.searchSession = '';

    $scope.sortSessionTable = function(sortSessionType) {
      if ($scope.sortSessionType === sortSessionType) {
        $scope.sortSessionReverse = !$scope.sortSessionReverse;
      } else {
        $scope.sortSessionType = sortSessionType;
        $scope.sortSessionReverse = true;
      }
    };

    // User table sorting
    $scope.sortUserType = 'entry';
    $scope.sortUserReverse = true;
    $scope.searchUser = '';

    $scope.sortUserTable = function(sortUserType) {
      if ($scope.sortUserType === sortUserType) {
        $scope.sortUserReverse = !$scope.sortUserReverse;
      } else {
        $scope.sortUserType = sortUserType;
        $scope.sortUserReverse = true;
      }
    };

    $scope.showAlert = false;
    $scope.alertType = 'success';
    $scope.enableUpdate = true;
    $scope.disableStartGame = true;
    $scope.teamInformation = "";
    $scope.numOfSelectedUser = 0;
    $scope.selectedSession = false;
    $scope.selectedAddr = false;
    $scope.selectedMap = "";
    $scope.selectedGameMode = "";
    $scope.selectedDemoRecordType = "None";
    $scope.startupTime = 0;
    $scope.restartNum = 0;
    $scope.updateStats = 0;
    $scope.availableTeams = ["", "A", "B"];
    $scope.availableMaps = [
      "",
      "ThePlane",
      "LV901_mb_FVPoint_Daytime_a",
      "LV904_mb_Forest_Daytime_a",
      "LV906_mb_Gtoon_Daytime_a",
      "LV907_mb_Proto3_Daytime_a",
      "LV908_mb_PVPoint",
      "LV909_mb_ResourceMiningField_Daytime_a",
      "1on1_kari",
      "3on3_breakmap",
      "6on6_breakmap",
      "20vs40_kari",
      "breakmap_kokkyo",
      "LV910_JBR5_Daytime_a",
      "breakmap_kokkyo_ver2",
      "LV911_mb_RushField_Daytime_a",
      "PLV03_01_MoonSecurityTerminal",
      "PLV02_01_GuianaHighlands",
      "LV06_01_ilios_ruins"
    ];
    $scope.availableGameModes = [
      "",
      "FreeForAll",
      "TeamDeathMatch",
      "PointCapture",
      "Destruction",
      "Domination",
      "Headquarters",
    ];
    $scope.availableDemoRecordTypes = [
      "None",
      "LowQuality",
      "HighQuality",
    ];

    $scope.onChangeValues = function() {
      $scope.disableStartGame = true;
      if (!$scope.selectedSession || $scope.numOfSelectedUser <= 0) {
        return;
      }
      if (!$scope.selectedMap || !$scope.selectedGameMode) {
        return;
      }
      $scope.disableStartGame = false;
    };

    $scope.onFocusSelect = function() {
      $scope.enableUpdate = false;
    };

    $scope.onBlurSelect = function() {
      $scope.enableUpdate = true;
    };

    $scope.onChangeMap = function(session) {
      $scope.onChangeValues();
    };

    $scope.onChangeGameMode = function(session) {
      $scope.onChangeValues();
    };

    $scope.onChangeTeam = function(waiting) {
      var a = 0;
      var b = 0;
      _.forEach($scope.waitings, function(waiting) {
        if (waiting.selectedTeam === "A") {
          a += 1;
        } else if (waiting.selectedTeam === "B") {
          b += 1;
        }
      });
      $scope.teamInformation = "A " + a + " / B " + b;
      $scope.numOfSelectedUser = parseInt(a, 10) + b;
      $scope.onChangeValues();
    };

    $scope.onSelectSession = function(session) {
      $scope.selectedSession = session;
      $scope.onChangeValues();
    };

    $scope.onChangeDemoRecordType = function(waiting) {
    }

    $scope.onChangeStartupTime = function(waiting) {
    }

    $scope.onChangeUpdateStats = function(waiting) {
	}

    $scope.startGame = function() {
      $scope.disableStartGame = true;
      $scope.enableUpdate = false;
      var users = _.omit($scope.waitings, function(waiting, index) {
        if (waiting.selectedTeam) {
          return false;
        }
        return true;
      });
      var reset = function() {
        _.forEach($scope.waitings, function(waiting) {
          waiting.selectedTeam = "";
        });
        $scope.disableStartGame = false;
        $scope.teamInformation = "";
        $scope.selectedSession = false;
        $scope.selectedAddr = false;
        $scope.enableUpdate = true;
        $scope.getData();
      };
      Session.forceMatchmaking($scope.selectedSession, $scope.selectedMap, $scope.selectedGameMode, $scope.selectedDemoRecordType, $scope.startupTime, $scope.restartNum, $scope.updateStats, users)
        .then(function() {
          $scope.showAlert = true;
          $scope.alertType = 'success';
          $scope.message = 'マッチングが成功しました。';
          $scope.selectedMap = "";
          $scope.selectedGameMode = "";
          reset();
        })
        .catch(function(err) {
          $scope.showAlert = true;
          $scope.alertType = 'danger';
          $scope.message = 'マッチングが失敗しました。 : ' + err;
          reset();
        });
    };

    $scope.closeAlert = function() {
      $scope.showAlert = false;
      $scope.message = '';
    };

    $scope.getData = function() {
      if (!$scope.enableUpdate) {
        return;
      }

      $http.get('api/sessions/').then(function(res) {
        var sessions = _.omit(res.data, function(session, addr) {
          if (addr === 'hosts' || session.matching || session.gamemode.toLowerCase() !== 'matching') {
            return true;
          }
          session.addr = addr;
          session.servername2 = session.servername.substring(0, 20) + '...';
          return false;
        });

        $scope.sessions = [];
        _.forEach(sessions, function(session, addr) {
          $scope.sessions.push(session);
        });
      }, function(res) {
        console.log(res);
      });

      $http.get('api/sessions/waitings').then(function(res) {
        var waitings = _.omit(res.data, function(waiting, userId) {
          if (waiting.cancel || waiting.matching) {
            return true;
          }
          waiting.userId = userId;
          waiting.selectedTeam = "";
          return false;
        });

        _.forEach(waitings, function(waiting, userId) {
          var _waiting = _.find($scope.waitings, function(_waiting, index) {
            if (userId === _waiting.userId) {
              return true;
            }
            return false;
          });
          if (_waiting) {
            waiting.selectedTeam = _waiting.selectedTeam;
          }
        });

        $scope.waitings = [];
        _.forEach(waitings, function(waiting, userId) {
          $scope.waitings.push(waiting);
        });
      }, function(res) {
        console.log(res);
      });

      $scope.onChangeValues();
    }

    $scope.$on("$destroy", function() {
      $interval.cancel($scope.updateTimer);
    });
    $scope.updateTimer = $interval($scope.getData, 3000);
    $scope.getData();
  });
