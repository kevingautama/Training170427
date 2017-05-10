app.controller("GetOrder", function ($scope, WaiterResource)
{
    var waiterresource = new WaiterResource();
    $scope.GetOrder=[];
    WaiterResource.GetOrder(function (data) {
        console.log(data);
        $scope.GetOrder = data;
    });

    //$scope.GetOrder = function() {
    //    var service = RestaurantService.getOrder();
    //    service.then(function (d) {
    //        $scope.order = d.data;
    //    },function (error)
    //        {
    //        console.log("error");
    //        })  
    //}
    
    

    $scope.DetailOrder = function (id) {
        console.log(id);
    }
   
});
