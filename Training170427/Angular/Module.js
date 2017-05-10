
var services = angular.module("RestaurantAppServices", []);

services.factory("waiterResource",
    ["$resource",
        waiterResource]);

function waiterResource($resource) {
    return $resource("/api/WaiterAPI/:action/:id",
        { id: '@id' },
        {
            GetOrder: { method: 'GET', isArray: true },
        }
    )
}


var app = angular.module("RestaurantApp", ["ngResource", "RestaurantAppServices"]);

