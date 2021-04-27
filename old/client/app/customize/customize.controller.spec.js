'use strict';

describe('Controller: CustomizeCtrl', function () {

  // load the controller's module
  beforeEach(module('evoserverApp'));

  var CustomizeCtrl, scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    CustomizeCtrl = $controller('CustomizeCtrl', {
      $scope: scope
    });
  }));

  it('should ...', function () {
  });
});
