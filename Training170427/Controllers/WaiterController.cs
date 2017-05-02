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
        public ActionResult Index()
        {
            var order = db.Order.Include(o => o.Type);
            return View(order.ToList());
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

        

            return null;
        }

        // POST: Waiter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,TypeID,Finish,CreatedBy,CreatedDate,UpdatedBy,UpdateDate,IsDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.Type, "TypeID", "TypeName", order.TypeID);
            return View(order);
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
