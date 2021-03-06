﻿var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice,kitchenservice, $timeout) {
    var testService = new testservice();
    var kitchenService = new kitchenservice();
    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();
    $scope.Name = '';
    $scope.typeID = 0;
    $scope.tableID = 0;
    $scope.orderID = 0;
    $scope.isAddOrder = false;
    $scope.isEditMode = false;

    $scope.addOrder = function (id) {
        console.log(id);
        $scope.orderID = id;
        $scope.isAddOrder = true;
        //$scope.selectedOrder = angular.copy($scope.detailorder);
        $scope.GetMenu($scope.detailorder.TableID, $scope.detailorder.TableName, $scope.detailorder.TypeID);
    };

    $scope.DetailOrder = function (id) {
        console.log(id);
        $scope.isEditMode = false;
        $scope.pay = false;
        $scope.Name = '';
        testService.$DetailOrder({ id: id }, function (data) {
            $scope.test = false;
            $scope.detailorder = data;
            $scope.Name = data.Name;

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
        });
        $scope.tax = $scope.grandTotal * 0.1;
    };

    $scope.edit = function () {
        $scope.isEditMode = true;
        //console.log($scope.test);
    };

    $scope.save = function () {
        
        $scope.new = {
            "OrderID": $scope.detailorder.OrderID,
            "OrderItem": $scope.detailorder.OrderItem
        }
        //api post disini
        //console.log($scope.detailorder);
        testservice.EditOrder($scope.new, function (data) {
            $scope.new = {};
            $scope.isEditMode = false;
        })
        //console.log($scope.test);
    };      
    
    $scope.serve = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.ServedOrder({ id: orderItemId }, function (data) {
            if (data.Status === true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            }
            //$scope.status = data;
            console.log(data);
        });
        
    };

    $scope.Pay = function () {
        //console.log(id + " Triggered");
        //$scope.pay = {};
        //$scope.pay = $scope.detailorder;

        //console.log($scope.pay);
        $scope.pay = true;
        angular.forEach($scope.detailorder.OrderItem, function (item) {
            if (item.Status !== 'Served') {
                console.log(item.Status);
                $scope.pay = false;
            }
        });
        if ($scope.pay === false) {
            alert('semua makanan belum dihidang');
        }
       
    };
    $scope.CancelPay = function () {
     
        $scope.pay = false;
    };

    function print(div) {
        var printContents = document.getElementById(div).innerHTML;
        var popupWin = window.open("", "");
       
        popupWin.document.write('<html><head><title>Restaurant</title>'
            + '<link href="/Content/bootstrap.css" rel="stylesheet" />'
            + '</head><body onload="window.print()">' + printContents + '</body></html>');
        popupWin.document.close();
    };

    $scope.GoPay = function (id, uang, total) {
       
        console.log('DetailOrder');
        console.log(id + "," + uang + "," + total);
        if (uang >= total) {
            console.log("uang cukup");
            testservice.PayOrder({ id: id }, function (data) {
                if (data.Status === true) {
                    console.log("Success");
                    print('DetailOrder');
                    $scope.order = testservice.GetOrder();
                    $scope.detailorder = null;

                } else {
                    console.log("Failed");
                }
            });
        } else {
            console.log("uang tidak cukup");
            alert('uang tidak mencukupi');
        }
    };

    $scope.cancel = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.CancelOrder({ id: orderItemId }, function (data) {
            if (data.Status === true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            }
            //$scope.status = data;
            console.log(data);
        });

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

        $scope.orderedItems = [];
        //$scope.Name = '';
        //
        $scope.tableID = id;
        $scope.typeID = typeid;

        if (!$scope.isAddOrder) {
            $scope.Name = '';
            //$scope.orderedItems = $scope.selectedOrder.OrderItem;
        }

        $scope.baru = { "TableID": id, "TableName": tablename };
        //
        console.log("tes");
        testservice.GetMenu({}, function (data) {
            $scope.menu = data;

            console.log('menu',data);
        })
    };

    $scope.orderedItems = [];
    console.log($scope.orderedItems);

    $scope.addqty = function (item) {
 
        if (item.Notes == null)
            item.Notes = '';

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

    $scope.EditQtyPlus = function (index) {

        $scope.detailorder.OrderItem[index].Qty++;        

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
    };

    $scope.EditQtyMinus = function (index) {  
        if ($scope.detailorder.OrderItem[index].Qty > 1) {
            $scope.detailorder.OrderItem[index].Qty--;     
        };
    };

    $scope.new = {}
    $scope.CreateOrder = function () {
        //console.log($s, tableid)

        // Ini untuk add order
        if ($scope.isAddOrder) {

            // object yg d post
            //console.log($scope.selectedOrder);
            $scope.new = {
                "OrderID": $scope.orderID,
                "OrderItem": $scope.orderedItems
            }
            //api post disini
            console.log($scope.orderedItems);

            testservice.AddOrder($scope.new, function (data) {
                console.log($scope.new);
                console.log(data);
                $scope.order = testservice.GetOrder();
                $scope.detailorder = null;
                $scope.isAddOrder = false;
                $scope.selectedOrder = {};
                $scope.new = {};
            })

            // kosongin
            //$scope.isAddOrder = false;
            //$scope.selectedOrder = {};

        } else { // create order

            $scope.new = {
                "Name": $scope.Name,
                "TypeID": $scope.typeID,
                "TableID": $scope.tableID,
                "OrderItem": $scope.orderedItems
            }
            console.log($scope.new);
            //testservice.data = $scope.new;
            testservice.NewOrder($scope.new, function (data) {
                console.log(data);
                $scope.order = testservice.GetOrder();
                $scope.detailorder = null;
                $scope.new = {};
            })
        }
    };

    //----------------------------------------Kitchen------------------------------------------------------------

    $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();

    $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
    $scope.kitchenorder = kitchenservice.GetAllOrder();

    $scope.CancelOrderItem = function (id) {
        console.log(id);
        kitchenservice.CancelOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            }
        });

    };

    $scope.CookOrderItem = function (id) {
        console.log(id);
        kitchenservice.CookOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        });

    };

    $scope.FinishOrderItem = function (id) {
        console.log(id);
        kitchenservice.FinishOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        });

    };

    $scope.GetOrderItemByOrderID = function (id) {
        console.log(id);
        $scope.orderitem = kitchenservice.GetOrderItemByOrderID({ id: id });
        console.log($scope.orderitem);
    };

    $scope.kitchenprint = {};
    $scope.printkitchen = function (id) {
       
        console.log(id);
        kitchenservice.GetOrderItemPrint({ id: id }, function (obj) {
            $scope.kitchenprint = obj;
            console.log($scope.kitchenprint);
            $timeout(function () {
                print('printkitchen');
            }, 500);
           

        });
        
    }

    


});