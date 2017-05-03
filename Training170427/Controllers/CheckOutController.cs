using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Training170427.Controllers
{
    public class CheckOutController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();
        // GET: CheckOut
        public ActionResult Index()
        {
            var data = (from a in db.Order
                       where a.Finish == false && a.IsDeleted != true
                       select a).ToList();
            
            foreach(var item in data)
            {
                
            }
            return View();
        }
    }
}