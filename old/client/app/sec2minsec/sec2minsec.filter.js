'use strict';

angular.module('evoserverApp')
  .filter('sec2minsec', function () {
    return function (input) {
      return ('0' + Math.floor(input / 60)).slice(-2) + ':' + ('0' + Math.floor(input % 60)).slice(-2);
    };
  });
