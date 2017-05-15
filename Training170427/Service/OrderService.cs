﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Training170427.Models;

namespace Training170427.Service
{
    public class OrderService
    {
        private RestaurantEntities db = new RestaurantEntities();
        public List<OrderType> GetOrder()
        {
            List<OrderType> listdata = new List<OrderType>();

            listdata = (from a in db.Type
                        where a.IsDeleted != true
                        select new Models.OrderType
                        {
                            TypeID = a.TypeID,
                            TypeName = a.TypeName
                        }).ToList();

            foreach (var item in listdata)
            {
                List<Models.Order> listorder = new List<Models.Order>();

                var order = (from a in db.Order
                            where a.IsDeleted != true && a.Finish != true && a.TypeID == item.TypeID
                            select a).ToList();

                foreach (var item2 in order)
                {
                    Models.Order data = new Models.Order();
                    var table = (from a in db.Track
                                 where a.OrderID == item2.OrderID
                                 select a).FirstOrDefault();
                    data.OrderID = item2.OrderID;
                    data.Name = item2.Name;
                    if (table != null)
                    {
                        data.TableID = table.TableID;
                        data.TableName = table.Table.TableName;
                    }
                    data.TypeID = item2.TypeID;
                    data.OrderDate = item2.CreatedDate;


                    var orderitem = from a in db.OrderItem
                                    where a.IsDeleted != true && a.OrderID == item2.OrderID && a.Status != "Cancel"
                                    select a;
                    var i = 0;
                    foreach (var item3 in orderitem)
                    {
                        if (item3.Status == "Served")
                        {
                            i++;
                        }
                    }
                    data.OrderServed = i + "/" + orderitem.Count();
                    listorder.Add(data);
                }

                item.Order = listorder;
            }

            return listdata;
        }

        public Models.Order DetailOrder(int id)
        {
            Models.Order data = new Models.Order();
            var order = db.Order.Find(id);

            data.OrderID = order.OrderID;
            data.OrderDate = order.CreatedDate;
            data.TypeID = order.TypeID;

            if (db.Type.Find(order.TypeID).TypeName == "Order")
            {
                var table = (from a in db.Track
                             where a.OrderID == id
                             select a).FirstOrDefault();

                data.TableID = table.TableID;
                data.TableName = table.Table.TableName;
            }

            var orderitem = (from a in db.OrderItem
                            where a.IsDeleted != true && a.OrderID == data.OrderID && a.Status != "Cancel"
                            select new OrderItemViewModel
                            {
                                OrderItemID = a.OrderItemID,
                                MenuID = a.MenuID,
                                MenuName = a.Menu.MenuName,
                                Status=a.Status,
                                Price = a.Menu.MenuPrice,
                                Qty = a.Qty
                            }).ToList();

            data.OrderItem = orderitem;
            return data;
        }

        public List<Models.TableViewModel> Table()
        {
            var table = (from a in db.Table
                         where a.IsDeleted != true && a.TableStatus == "NotOccupied"
                         select new TableViewModel
                         {
                             TableID = a.TableID,
                             TableName = a.TableName
                         }).ToList();
            return table;
        }

        //public Models.AddOrder ChooseMenu(int? TableID)
        //{
        //        AddOrder order = new AddOrder();
        //        order.TableID = TableID;
        //        var category = (from a in db.Category
        //                        where a.IsDeleted != true
        //                        select a);
        //        List<CategoryViewModel> ListCategory = new List<CategoryViewModel>();
        //        foreach (var item in category)
        //        {
        //            CategoryViewModel Category = new CategoryViewModel();
        //            Category.CategoryID = item.CategoryID;
        //            Category.CategoryName = item.CategoryName;
        //            var listmenu = (from a in db.Menu
        //                            where a.IsDeleted != true && a.CategoryID == item.CategoryID && a.Status.StatusName == "Ready"
        //                            select new OrderItemViewModel
        //                            {
        //                                MenuID = a.MenuID,
        //                                MenuName = a.MenuName,
        //                                Price = a.MenuPrice,
        //                                Content = a.Content,
        //                                ContentType = a.ContentType
        //                            }).ToList();
        //            Category.OrderItem = listmenu;
        //            ListCategory.Add(Category);
        //        }
        //        order.Category = ListCategory;
        //        return order;
        //}
    }
}