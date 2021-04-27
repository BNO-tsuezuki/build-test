'use strict';

describe('Controller: GamestatDetailsCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var GamestatDetailsCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    GamestatDetailsCtrl = $controller('GamestatDetailsCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
