'use strict';

describe('Controller: PerformancestatCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var PerformancestatCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    PerformancestatCtrl = $controller('PerformancestatCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
