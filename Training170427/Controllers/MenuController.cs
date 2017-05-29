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
    public class MenuController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();

        // GET: Menu
        public ActionResult Index()
        {
           
            var menu = db.Menu.Include(m => m.Category).Include(m => m.Status).Where(a => a.IsDeleted != true);
            return View(menu.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Category.Where(a => a.IsDeleted != true), "CategoryID", "CategoryName");
            ViewBag.StatusID = new SelectList(db.Status.Where(a => a.IsDeleted != true), "StatusID", "StatusName");
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuID,MenuName,MenuPrice,MenuDescription,Content,ContentType,CategoryID,StatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Menu menu, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                menu.ContentType = upload.ContentType;
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {

                    menu.Content = reader.ReadBytes(upload.ContentLength);
                }
                menu.CreatedBy = "Admin";
                menu.CreatedDate = DateTime.Now;
                db.Menu.Add(menu);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category.Where(a => a.IsDeleted != true), "CategoryID", "CategoryName", menu.CategoryID);
            ViewBag.StatusID = new SelectList(db.Status.Where(a => a.IsDeleted != true), "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", menu.CategoryID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuID,MenuName,MenuPrice,MenuDescription,Content,ContentType,CategoryID,StatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Menu menu, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                Menu data = new Menu();
                data = db.Menu.Find(menu.MenuID);
                data.MenuName = menu.MenuName;
                data.MenuPrice = menu.MenuPrice;
                data.MenuDescription = menu.MenuDescription;
                data.UpdatedBy = "Admin";
                data.UpdatedDate = DateTime.Now;
                data.StatusID = menu.StatusID;
                data.CategoryID = menu.CategoryID;

                if (upload != null)
                {
                    data.ContentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        data.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }   
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", menu.CategoryID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", menu.StatusID);
            return View(menu);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Find(id);
            db.Menu.Remove(menu);
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

        public ActionResult GetFile(int id)
        {
            var fileToRetrieve = db.Menu.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}
