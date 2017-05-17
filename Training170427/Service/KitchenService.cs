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
                                    OrderID = a.OrderID
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
    }
}