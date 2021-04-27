'use strict';

describe('Controller: MobilesuitCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var MobilesuitCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    MobilesuitCtrl = $controller('MobilesuitCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
    expect(1).toEqual(1);
  });
});
