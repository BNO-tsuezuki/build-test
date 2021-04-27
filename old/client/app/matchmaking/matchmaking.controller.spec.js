'use strict';

describe('Controller: MatchmakingCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var MatchmakingCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    MatchmakingCtrl = $controller('MatchmakingCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
