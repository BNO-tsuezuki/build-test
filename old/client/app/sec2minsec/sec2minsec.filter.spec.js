'use strict';

describe('Filter: sec2minsec', function () {

  // load the filter's module
  beforeEach(module('evoserverApp'));

  // initialize a new instance of the filter before each test
  var sec2minsec;
  beforeEach(inject(function ($filter) {
    sec2minsec = $filter('sec2minsec');
  }));

  it('should filtered', function () {
    var input;
    input = 198.56789;
    expect(sec2minsec(input)).toBe('03:18');
  });

});
