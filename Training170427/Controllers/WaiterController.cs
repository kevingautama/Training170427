using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Training170427;
using Training170427.Models;

namespace Training170427.Controllers
{
    public class WaiterController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Waiter
        //public ActionResult Index()
        //{
        //    var order = db.Order.Include(o => o.Type);
        //    return View(order.ToList());
        //}

        public ActionResult Index()
        {
            var status = 0;
            var order = (from a in db.Order
                         where a.Finish == false && a.IsDeleted == false
                         select new AddOrder
                         {
                             OrderID = a.OrderID,
                             TypeName = a.Type.TypeName,
                             TableID = (from b in db.Track
                                        where b.OrderID == a.OrderID
                                        select b.TableID).FirstOrDefault(),
                             TableName = (from b in db.Track
                                        where b.OrderID == a.OrderID
                                        select b.Table.TableName).FirstOrDefault()
                            
                         }).ToList();

            foreach(var item in order)
            {
               
                var orderitem = (from a in db.OrderItem
                                where a.OrderID == item.OrderID
                                select a.Status).ToList();
                foreach(var item2 in orderitem)
                {
                    if(item2 == "FinishCook")
                    {
                        status = status + 1;
                    }
                }
                item.Status = status;
            }
            return View(order);
        }

        // GET: Waiter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Waiter/Create
        public ActionResult Create()
        {
            ViewBag.TypeID = new SelectList(db.Type, "TypeID", "TypeName");
            return View();
        }

        [HttpPost]
        public ActionResult ChooseTable(int TypeID)
        {
            AddOrder data = new AddOrder();
            data.TypeID = TypeID;
            if(db.Type.Find(TypeID).TypeName == "TakeAway")
            {
                return RedirectToAction("ChooseMenu",data);
            }
            else
            {
                List<Table> table = new List<Table>();
                table = db.Table.Where(a => a.IsDeleted != true && a.TableStatus == "NotOccupied").ToList();
                data.Table = table;
                return View(data);
                
            }
        }

        //[HttpPost]
        public ActionResult ChooseMenu(int TypeID, int? TableID)
        {
            AddOrder data = new AddOrder();
            data.TypeID = TypeID;
            data.TableID = TableID;

            var category = from a in db.Category
                           where a.IsDeleted != true
                           select a;
           
            List<CategoryViewModel> ListCategory = new List<CategoryViewModel>();
            foreach(var item in category)
            {
                CategoryViewModel Category = new CategoryViewModel();
                Category.CategoryID = item.CategoryID;
                Category.CategoryName = item.CategoryName;
                var listmenu = (from a in db.Menu
                                    where a.CategoryID == item.CategoryID && a.Status.StatusName =="Ready"
                                    select new OrderItemViewModel
                                    {
                                        MenuID = a.MenuID,
                                        MenuName = a.MenuName,
                                        Price = a.MenuPrice,
                                        Content = a.Content,
                                        ContentType = a.ContentType
                                    }).ToList();
                Category.OrderItem = listmenu;
                ListCategory.Add(Category);
            }
     
            data.Category = ListCategory;
               
                             
        

            return View(data);
        }

        // POST: Waiter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "OrderID,OrderDate,TypeID,Finish,CreatedBy,CreatedDate,UpdatedBy,UpdateDate,IsDeleted")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Order.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.TypeID = new SelectList(db.Type, "TypeID", "TypeName", order.TypeID);
        //    return View(order);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddOrder data)
        {
            var Type = db.Type.Find(data.TypeID).TypeName;
            if(Type == "Order")
            {
                if(db.Table.Find(data.TableID).TableStatus == "NotOccupied")
                {
                    Table table = db.Table.Find(data.TableID);
                    table.TableStatus = "Occupied";
                    db.Entry(table).State = EntityState.Modified;

                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    order.TypeID = data.TypeID;
                    order.Finish = false;
                    order.CreatedBy = "Admin";
                    order.CreatedDate = DateTime.Now;
                    order.IsDeleted = false;
                    db.Order.Add(order);

                    foreach(var item in data.Category)
                    {
                        foreach(var item2 in item.OrderItem)
                        {
                            if(item2.Qty > 0)
                            {
                                OrderItem orderitem = new OrderItem();
                                orderitem.OrderID = order.OrderID;
                                orderitem.MenuID = item2.MenuID;
                                orderitem.Qty = item2.Qty;
                                orderitem.Notes = item2.Notes;
                                orderitem.CreatedBy = "Admin";
                                orderitem.CreatedDate = DateTime.Now;
                                orderitem.Status = "Order";
                                db.OrderItem.Add(orderitem);
                            }
                        }
                    }

                    Track track = new Track();
                    track.OrderID = order.OrderID;
                    track.TableID = table.TableID;
                    track.CreatedBy = "Admin";
                    track.CreatedDate = DateTime.Now;
                    db.Track.Add(track);

                }
                else
                {
                    return RedirectToAction("Create");
                }
            }else if(Type == "TakeAway")
            {
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.TypeID = data.TypeID;
                order.Finish = false;
                order.CreatedBy = "Admin";
                order.CreatedDate = DateTime.Now;
                order.IsDeleted = false;
                db.Order.Add(order);

                foreach (var item in data.Category)
                {
                    foreach (var item2 in item.OrderItem)
                    {
                        if (item2.Qty > 0)
                        {
                            OrderItem orderitem = new OrderItem();
                            orderitem.OrderID = order.OrderID;
                            orderitem.MenuID = item2.MenuID;
                            orderitem.Qty = item2.Qty;
                            orderitem.Notes = item2.Notes;
                            orderitem.Status = "Order";
                            orderitem.CreatedBy = "Admin";
                            orderitem.CreatedDate = DateTime.Now;
                            db.OrderItem.Add(orderitem);
                        }
                    }
                }
            }
            db.SaveChanges();
            return null;
        }

        // GET: Waiter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db.Type, "TypeID", "TypeName", order.TypeID);
            return View(order);
        }

        // POST: Waiter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,TypeID,Finish,CreatedBy,CreatedDate,UpdatedBy,UpdateDate,IsDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.Type, "TypeID", "TypeName", order.TypeID);
            return View(order);
        }

        // GET: Waiter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Waiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
