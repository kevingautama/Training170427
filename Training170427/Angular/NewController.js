var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice) {
    var testService = new testservice();
    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();

    $scope.DetailOrder = function (id) {
        console.log(id);
        testService.$DetailOrder({ id: id }, function (data) {
            $scope.test = false;
            $scope.detailorder = data;
            console.log($scope.detailorder);
        });
    };

    $scope.edit = function () {
        $scope.test = true;
        console.log($scope.test);
    };

    $scope.save = function () {
        $scope.test = false;
        console.log($scope.test);
    };
    
    $scope.serve = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.ServedOrder({ id: orderItemId }, function (data) {
            if (data.Status == true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            } else {

            }
            //$scope.status = data;
            console.log(data);
        })
        
    };

    $scope.cancel = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.CancelOrder({ id: orderItemId }, function (data) {
            if (data.Status == true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            } else {

            }
            //$scope.status = data;
            console.log(data);
        })

    };   

    
    $scope.GetTable = function () {       
        testservice.GetTable({}, function (data) {
            $scope.dataTable = data;
        });
        
    }

});