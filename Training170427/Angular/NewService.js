var service = angular.module('testService', []);

service.factory('testservice', function ($resource) {
    return $resource('/api/WaiterAPI/:action/:id', { id: '@id' }, {
        GetOrder: {
            method: 'GET', isArray:true
        },
        DetailOrder: {
            method: 'GET', params: { action: 'DetailOrder' }
        },
        ServedOrder: {
            method: 'POST' , params: {action: 'ServedOrder'}
        },
        //add table service
        GetTable: {
            method: 'GET', params: {action: 'GetTable' }
        }
    });
});

