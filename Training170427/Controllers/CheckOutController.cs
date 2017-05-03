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
    }
}