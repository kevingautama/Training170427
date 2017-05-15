var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice) {
    var testService = new testservice();

    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();

    $scope.DetailOrder = function (id) {
        console.log(id);
        testService.$DetailOrder({ id: id }, function (data) {
            $scope.test = false;
            $scope.detailorder = data;

            $scope.calculateGrandTotal();

            console.log($scope.detailorder);
        });
    };

    $scope.calculateGrandTotal = function () {
        $scope.grandTotal = 0;
        $scope.tax = 0;
        console.log('Triggered');

        angular.forEach($scope.detailorder.OrderItem, function (item) {
            console.log('Triggered 1');
            $scope.grandTotal = $scope.grandTotal + (item.Qty * item.Price);
            console.log(item);
        })
        $scope.tax = $scope.grandTotal * 0.1;
    }

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

    $scope.Pay = function () {
        //console.log(id + " Triggered");
        //$scope.pay = {};
        //$scope.pay = $scope.detailorder;

        //console.log($scope.pay);
        $scope.pay = true;
    };
    $scope.CancelPay = function () {
     
        $scope.pay = false;
    };

    $scope.GoPay = function (id,uang,total) {
        
        console.log(id + "," + uang +","+total);
        if (uang > total) {
            console.log("uang cukup");
            testservice.PayOrder({ id: id }, function (data) {
                if (data.Status == true) {
                    console.log("Success");
                    $scope.order = testservice.GetOrder();
                    $scope.detailorder = {};
                } else {
                    console.log("Failed");
                }
            })
        } else {
            console.log("uang tidak cukup");
        }
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
            console.log(data);
        });        
    }

});