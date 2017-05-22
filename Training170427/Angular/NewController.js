var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice,kitchenservice) {
    var testService = new testservice();
    var kitchenService = new kitchenservice();
    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();

    $scope.DetailOrder = function (id) {
        console.log(id);
        $scope.pay = false;
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
        angular.forEach($scope.detailorder.OrderItem, function (item) {
            if (item.Status != 'Served') {
                console.log(item.Status)
                $scope.pay = false;
            }
        })
        if ($scope.pay == false) {
            alert('semua makanan belum dihidang');
        }
       
    };
    $scope.CancelPay = function () {
     
        $scope.pay = false;
    };

    $scope.GoPay = function (id,uang,total) {
        
        console.log(id + "," + uang + "," + total);
        if (uang >= total) {
            console.log("uang cukup");
            testservice.PayOrder({ id: id }, function (data) {
                if (data.Status == true) {
                    console.log("Success");
                    $scope.order = testservice.GetOrder();
                    $scope.detailorder = null;
                } else {
                    console.log("Failed");
                }
            })
        } else {
            console.log("uang tidak cukup");
            alert('uang tidak mencukupi');
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

    
    $scope.GetTable = function ()
    {
        testservice.GetTable({}, function (data)
        {
            $scope.dataTable = data;
            console.log(data);
        });        
    };

    $scope.Category = [
        {
            "CategoryID": 1,
            "CategoryName": "Food",
            "Menu": [
                { "MenuID": 1, "MenuName": "Nasi Goreng", "Qty": 2, "CategoryID": 1, "MenuPrice": 10000 },
                { "MenuID": 2, "MenuName": "Mie Goreng", "Qty": 2, "CategoryID": 1, "MenuPrice": 10000 }
            ]
        },
        {
            "CategoryID": 2,
            "CategoryName": "Drink",
            "Menu": [
                { "MenuID": 3, "MenuName": "Ice tea", "Qty": 2, "CategoryID": 2, "MenuPrice": 10000 },
                { "MenuID": 4, "MenuName": "teh tarek", "Qty": 2, "CategoryID": 2, "MenuPrice": 10000 }
            ]
        }
    ];
    $scope.orderedItems = [];
    console.log($scope.orderedItems);

    $scope.addqty = function (item) {
        $scope.cek = false;
        angular.forEach($scope.orderedItems, function (obj) {
            if (item.MenuID == obj.MenuID) {
                $scope.cek = true;
                obj.Qty = obj.Qty + 1;
            } 
        })
        if ($scope.cek == false) {
            $scope.orderedItems.push(item);
        }
    };

    $scope.delqty = function (MenuID, index) {
        console.log(MenuID);
        angular.forEach($scope.orderedItems, function (obj) {
            if (MenuID == obj.MenuID) {
                $scope.cek = true;
                if (obj.Qty == 1) {
                    $scope.orderedItems.splice(index, 1);
                } else {
                    obj.Qty = obj.Qty - 1;
                }                                              
            }
        })
    }



    //----------------------------------------Kitchen------------------------------------------------------------

    $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();

    $scope.CancelOrderItem = function (id) {
        console.log(id);
        kitchenservice.CancelOrderItem({ id: id }, function (data) {
            if (data.Status == true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            }
        })
       
    }

    $scope.CookOrderItem = function (id) {
        console.log(id);
        kitchenservice.CookOrderItem({ id: id }, function (data) {
            if (data.Status == true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            }
        })
       
    }

    $scope.FinishOrderItem = function (id) {
        console.log(id);
        kitchenservice.FinishOrderItem({ id: id }, function (data) {
            if (data.Status == true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            }
        })
        
    }


});