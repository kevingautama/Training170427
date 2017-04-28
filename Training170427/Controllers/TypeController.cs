using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Training170427;

namespace Training170427.Controllers
{
    public class TypeController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Type
        public ActionResult Index()
        {
            return View(db.Type.ToList().Where(a=> a.IsDeleted !=true));
        }

        // GET: Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Type type = db.Type.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // GET: Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Type type)
        {
            if (ModelState.IsValid)
            {
                type.CreatedBy = "Admin";
                type.CreatedDate = DateTime.Now;
                db.Type.Add(type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(type);
        }

        // GET: Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Type type = db.Type.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Type type)
        {
            if (ModelState.IsValid)
            {
                Type data = new Type();
                data = db.Type.Find(type.TypeID);
                data.TypeName = type.TypeName;
                data.UpdatedBy = "Admin";
                data.UpdatedDate = DateTime.Now;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(type);
        }

        // GET: Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Type type = db.Type.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Type type = db.Type.Find(id);
            type.IsDeleted = true;
            db.Entry(type).State = EntityState.Modified;
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
