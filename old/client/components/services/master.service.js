'use strict';

angular.module('evoserverApp')
  .factory('Master', function ($http, $q, $resource) {
    var MasterResource = $resource('/api/master/:controller/:id', {}, {
      mobilesuitsAsObject: {
        method: 'GET',
        params: {
          controller: 'mobilesuits',
          type: 'object'
        }
      },
      mobilesuitsAsArray: {
        method: 'GET',
        params: {
          controller: 'mobilesuits'
        },
        isArray: true
      },
      equipmentsAsObject: {
        method: 'GET',
        params: {
          controller: 'equipments',
          type: 'object'
        }
      },
      equipmentsAsArray: {
        method: 'GET',
        params: {
          controller: 'equipments'
        },
        isArray: true
      },
      devtreeAsObject: {
        method: 'GET',
        params: {
          controller: 'devtree',
          type: 'object'
        }
      },
      devtreeAsArray: {
        method: 'GET',
        params: {
          controller: 'devtree'
        },
        isArray: true
      }
    });

    return {
      mobilesuitsAsObject: function (params) {
        return MasterResource.mobilesuitsAsObject(params).$promise;
      },
      mobilesuitsAsArray: function (params) {
        return MasterResource.mobilesuitsAsArray(params).$promise;
      },
      equipmentsAsObject: function (params) {
        return MasterResource.equipmentsAsObject(params).$promise;
      },
      equipmentsAsArray: function (params) {
        return MasterResource.equipmentsAsArray(params).$promise;
      },
      devtreeAsObject: function (params) {
        return MasterResource.devtreeAsObject(params).$promise;
      },
      devtreeAsArray: function (params) {
        return MasterResource.devtreeAsArray(params).$promise;
      }
    };

  });
