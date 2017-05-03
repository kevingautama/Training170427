using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Training170427.Controllers
{
    public class KitchenController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();
        // GET: Kitchen
        public ActionResult Index()
        {
            var order = (from a in db.Order
                        where a.Finish == false && a.IsDeleted == false
                        select a).ToList();


            return View();
        }
    }
}