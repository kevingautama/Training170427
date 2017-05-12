var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice) {
    var testService = new testservice();
    $scope.detailorder = {};
    $scope.order = testservice.GetOrder(); //fetch all movies. Issues a GET to /api/movies
    $scope.DetailOrder = function (id) {
        console.log(id);
        testService.$DetailOrder({ id: id }, function (data)
        {
            
            $scope.detailorder = data;
            console.log($scope.detailorder);
        });
    }

    $scope.table = {};

    $scope.GetTable = function () {
        console.log("masuk");
        testservice.$GetTable(), function (data) {
            //$scope.table = data;
            console.log(data);
        }
    }

    
});