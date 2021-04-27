'use strict';

describe('Controller: GamestatCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var GamestatCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    GamestatCtrl = $controller('GamestatCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
