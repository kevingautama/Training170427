using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Training170427.Models;

namespace Training170427.Controllers
{
    public class CheckOutController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();
        // GET: CheckOut
        public ActionResult Index()
        {
            List<AddOrder> listdata = new List<AddOrder>();
            var data = (from a in db.Order
                       where a.Finish == false && a.IsDeleted != true
                       select a).ToList();
            
            foreach(var item in data)
            {
                AddOrder order = new AddOrder();
                var orderitem = (from a in db.OrderItem
                                where a.OrderID == item.OrderID && a.IsDeleted != true && a.Status != "Cancel"
                                select a.Status).ToList();
                var status = 0;

                foreach(var item2 in orderitem)
                {
                    if(item2 == "FinishCook")
                    {
                        status = status + 1;
                    }
                }
                
                if(status == orderitem.Count)
                {
                    order.OrderID = item.OrderID;
                    order.TableName = (from a in db.Track
                                       where a.OrderID == item.OrderID
                                       select a.Table.TableName).FirstOrDefault();
                    order.TypeName = item.Type.TypeName;

                    listdata.Add(order);

                }
            }
            return View(listdata);
        }

        public ActionResult CheckOutOrder(int id)
        {
            CheckOutModel data = new CheckOutModel();
            var order = db.Order.Find(id);
            if(order != null)
            {
                var tablename = (from a in db.Track
                                 where a.OrderID == id
                                 select a).FirstOrDefault();
                var orderitem = (from a in db.OrderItem
                                 where a.OrderID == id && a.Status == "FinishCook"
                                 select new OrderItemViewModel
                                 {
                                     MenuID = a.MenuID,
                                     MenuName = a.Menu.MenuName,
                                     Qty = a.Qty,
                                     

                                 }).ToList();
                double grandtotal = 0;
                foreach(var item in orderitem)
                {

                    item.Price = Convert.ToString(item.Qty * Convert.ToDecimal(db.Menu.Find(item.MenuID).MenuPrice));
                    grandtotal = grandtotal + Convert.ToDouble(item.Price);
                }

                data.OrderID = id;
                data.TableName = tablename.Table.TableName;
                data.GrandTotal = Convert.ToDecimal(grandtotal);
                data.OrderItem = orderitem;
                return View(data);
            }
            else
            {
                return null;
            }
        }
    }
}