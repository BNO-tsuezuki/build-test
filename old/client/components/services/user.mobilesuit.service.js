'use strict';

angular.module('evoserverApp')
  .factory('UserMobilesuit', function ($resource) {
    return $resource('/api/user/mobilesuits/:id/:controller', {},
    {
      disposal: {
        method: 'DELETE'
      },

      updateInventory: {
        method: 'PUT',
        params: {
          controller: 'inventory'
        }
      }
	  });
  });
