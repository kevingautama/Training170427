using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Training170427.Models;

namespace Training170427.Controllers
{
    public class KitchenController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();
        // GET: Kitchen
        public ActionResult Index()
        {
            List<AddOrder> listdata = new List<AddOrder>();
            
            var order = (from a in db.Order
                        where a.Finish == false && a.IsDeleted == false
                        select a).ToList();
            List<Category> OrderCategory = new List<Category>();
            foreach(var item in order)
            {
                AddOrder data = new AddOrder();
                List<OrderItemViewModel> listorderitem = new List<OrderItemViewModel>();

                var hasil = from a in db.OrderItem
                                where a.OrderID == item.OrderID && a.Status != "FinishCook" && a.Status !="Cancel" && a.IsDeleted != true && a.Status !="Served" && a.Status != "Paid"
                                select new OrderItemViewModel
                                {
                                    OrderItemID = a.OrderItemID,
                                    MenuID = a.MenuID,
                                    MenuName =a.Menu.MenuName,
                                    Qty = a.Qty,
                                    Status = a.Status,
                                    Notes =a.Notes
                                };
                foreach(var item2 in hasil)
                {
                    listorderitem.Add(item2);
                }

                data.OrderID = item.OrderID;
                data.TypeName = item.Type.TypeName;
                if(item.Type.TypeName == "Order")
                {
                    data.TableName = (from a in db.Track
                                      where a.OrderID == item.OrderID
                                      select a.Table.TableName).FirstOrDefault();
                }
                data.OrderItem = listorderitem;

                if(data.OrderItem.Count > 0)
                {
                    listdata.Add(data);
                }
                
            }
            return View(listdata);
        }

        public ActionResult cookitem(int id)
        {
            var data = db.OrderItem.Find(id);
            data.Status = "Cook";
            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult cancelitem(int id)
        {
            var data = db.OrderItem.Find(id);
            data.IsDeleted = true;
            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult finishitem(int id)
        {
            var data = db.OrderItem.Find(id);
            data.Status = "FinishCook";
            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult IndexAPI()
        {
            return View();
        }
    }
}
