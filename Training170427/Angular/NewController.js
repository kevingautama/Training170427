var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice,kitchenservice) {
    var testService = new testservice();
    var kitchenService = new kitchenservice();
    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();

    $scope.typeID = 0;
    $scope.tableID = 0;

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
    
    $scope.GetTable = function (typeid)
    {
        $scope.typeID = typeid;
        testservice.GetTable({}, function (data)
        {
            $scope.dataTable = data;
            console.log(data);
        });        
    };
    
    $scope.baru = {};
    $scope.GetMenu = function (id, tablename, typeid) {
        //
        $scope.tableID = id;
        $scope.typeID = typeid;

        $scope.baru = { "TableID": id, "TableName": tablename };
        $scope.orderedItems = [];
        //
        console.log("tes");
        testservice.GetMenu({}, function (data) {
            $scope.menu = data;
            console.log(data);
        })
    };

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
            item.Qty = item.Qty + 1;
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

    $scope.new = {}
    $scope.CreateOrder = function () {
        //console.log($s, tableid)
        $scope.new = { "TypeID": $scope.typeID, "TableID": $scope.tableID, "OrderItem": $scope.orderedItems }
        console.log($scope.new);
        //testservice.data = $scope.new;
        testservice.NewOrder($scope.new, function (data) {
            console.log(data);
            $scope.order = testservice.GetOrder();
            $scope.detailorder = null;
        })
    };
    // create function order
    // API post model order
    
    // create function

    //$scope.newOrder = {
    //    TableID : tableid,
    //    TypeID : typeid,
    //    OrderItem : $scope.orderedItems
    //};

    //----------------------------------------Kitchen------------------------------------------------------------

    $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();

    $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
    $scope.kitchenorder = kitchenservice.GetAllOrder();

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
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        })
       
    }

    $scope.FinishOrderItem = function (id) {
        console.log(id);
        kitchenservice.FinishOrderItem({ id: id }, function (data) {
            if (data.Status == true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        })
        
    }

    $scope.GetOrderItemByOrderID = function (id) {
        console.log(id);
        $scope.orderitem = kitchenservice.GetOrderItemByOrderID({ id: id });
        console.log($scope.orderitem);
    }

    


});