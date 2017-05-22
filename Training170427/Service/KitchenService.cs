using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Training170427.Models;

namespace Training170427.Service
{
    public class KitchenService
    {
        private RestaurantEntities db = new RestaurantEntities();

        public List<KitchenViewModel> GetAllOrderItem()
        {
            List<KitchenViewModel> listdata = new List<KitchenViewModel>();

            listdata.Add(new KitchenViewModel() {Status="Order"});
            listdata.Add(new KitchenViewModel() { Status = "Cook" });

           
            foreach(var item in listdata)
            {
               
                var orderitem = (from a in db.OrderItem
                                where a.IsDeleted != true && a.Status != "Cancel" && a.Status != "FinishCook" && a.Status != "Served" && a.Status != "Paid" && a.Status == item.Status
                                select new OrderItemViewModel
                                {
                                    OrderItemID = a.OrderItemID,
                                    MenuName = a.Menu.MenuName,
                                    Notes = a.Notes,
                                    Qty =a.Qty,
                                    OrderID = a.OrderID,
                                    Time = a.CreatedDate
                                }).ToList();
                foreach(var item2 in orderitem)
                {
                    var typename = (from a in db.Order
                                   where a.OrderID == item2.OrderID
                                   select a.Type.TypeName).FirstOrDefault();
                    item2.TypeName = typename;

                }
          
                item.OrderItem = orderitem;
            }
            return listdata;
        }

        public List<KitchenViewModel> GetAllOrderItemCateByOrder()
        {
            List<KitchenViewModel> listdata = new List<KitchenViewModel>();

            listdata.Add(new KitchenViewModel() { Status = "Order" });
            listdata.Add(new KitchenViewModel() { Status = "Cook" });


            var listorder = from a in db.Order
                        where a.IsDeleted != true && a.Finish != true
                        select a;

            foreach(var item in listdata)
            {
                List<Models.Order> listorderdata = new List<Models.Order>();
                foreach(var item2 in listorder)
                {
                    Models.Order order = new Models.Order();
                    order.OrderID = item2.OrderID;
                    order.TableName = (from a in db.Track
                                       where a.OrderID == item2.OrderID
                                       select a.Table.TableName).FirstOrDefault();
                    order.Name = item2.Name;
                    order.TypeName = item2.Type.TypeName;
                    var orderitem = (from a in db.OrderItem
                                    where a.IsDeleted != false && a.Status == item.Status && a.OrderID == item2.OrderID
                                    select new OrderItemViewModel
                                    {
                                        OrderItemID = a.OrderItemID,
                                        MenuName = a.Menu.MenuName,
                                        Notes = a.Notes,
                                        Time = a.CreatedDate,
                                        Qty = a.Qty
                                    }).ToList();
                    
                    order.OrderItem = orderitem;
                    listorderdata.Add(order);
                }
                item.Order = listorderdata;
            }
            return listdata;

        }

        public List<Models.Order> GetAllOrder()
        {
            var order = (from a in db.Order
                        where a.IsDeleted != true && a.Finish != true
                        select new Models.Order {
                            OrderID =a.OrderID ,
                            Name = a.Name,
                            OrderDate = a.CreatedDate,

                        }).ToList();
            foreach(var item in order)
            {
                item.TableName = (from a in db.Track
                                   where a.OrderID == item.OrderID
                                   select a.Table.TableName).FirstOrDefault();
            }

            return order;
        }

        public List<KitchenViewModel> GetOrderItemByOrderID(int id)
        {
            List<KitchenViewModel> listdata = new List<KitchenViewModel>();

            listdata.Add(new KitchenViewModel() { Status = "Order" });
            listdata.Add(new KitchenViewModel() { Status = "Cook" });

            foreach(var item in listdata)
            {
                var orderitem = (from a in db.OrderItem
                                 where a.IsDeleted != true && a.Status != "Cancel" && a.Status != "FinishCook" && a.Status != "Served" && a.Status != "Paid" && a.Status == item.Status && a.OrderID == id
                                 select new OrderItemViewModel
                                 {
                                     OrderItemID = a.OrderItemID,
                                     MenuName = a.Menu.MenuName,
                                     Notes = a.Notes,
                                     Qty = a.Qty,
                                     OrderID = a.OrderID,
                                     Time = a.CreatedDate
                                 }).ToList();

                foreach (var item2 in orderitem)
                {
                    var typename = (from a in db.Order
                                    where a.OrderID == item2.OrderID
                                    select a.Type.TypeName).FirstOrDefault();
                    item2.TypeName = typename;

                }

                item.OrderItem = orderitem;
            }
            return listdata;
        }

        
    }
}