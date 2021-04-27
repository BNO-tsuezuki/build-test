'use strict';

angular.module('evoserverApp')
  .factory('User', function ($resource) {
    return $resource('/api/users/:id/:controller', {
      id: '@_id'
    },
    {
      changePassword: {
        method: 'PUT',
        params: {
          controller:'password'
        }
      },
      updateDeck: {
        method: 'PUT',
        params: {
          controller:'deck'
        }
      },
      get: {
        method: 'GET',
        params: {
          id:'me'
        }
      },
      getDetail: {
        method: 'GET',
        params: {
          id:'me',
          type:'detail'
        }
      }
	  });
  });
