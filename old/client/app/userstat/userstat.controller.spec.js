'use strict';

describe('Controller: UserstatCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var UserstatCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    UserstatCtrl = $controller('UserstatCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
  });
});
