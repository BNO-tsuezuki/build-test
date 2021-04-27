'use strict';

describe('Controller: PerformancestatDetailsCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var PerformancestatDetailsCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    PerformancestatDetailsCtrl = $controller('PerformancestatDetailsCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
