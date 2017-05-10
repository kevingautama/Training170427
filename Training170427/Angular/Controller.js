app.controller("GetOrder", function ($scope, RestaurantService)
{
    getOrder();
    function getOrder() {
        var service = RestaurantService.getOrder();
        service.then(function (d) {
            $scope.order = d.data;
        },function (error)
            {
            console.log("error");
            })  
    }
   
});