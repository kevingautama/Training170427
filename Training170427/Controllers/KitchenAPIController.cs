using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Training170427.Models;
using Training170427.Service;

namespace Training170427.Controllers
{
    [RoutePrefix("api/KitchenAPI")]
    public class KitchenAPIController : ApiController
    {
        KitchenService service = new KitchenService();
        private RestaurantEntities db = new RestaurantEntities();
        [Route("GetAllOrderItem")]
        public List<KitchenViewModel> GetAllOrderItem()
        {
            return service.GetAllOrderItem();
        }

        [Route("CancelOrderItem/{id}")]
        public ResponseViewModel CancelOrderItem(int id)
        {
            var data = db.OrderItem.Find(id);

            if(data.Status == "Order")
            {
                data.Status = "Cancel";
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }
          
        }

        [Route("CookOrderItem/{id}")]
        public ResponseViewModel CookOrderItem(int id)
        {
            var data = db.OrderItem.Find(id);

            if (data.Status == "Order")
            {
                data.Status = "Cook";
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }

        }

        [Route("FinishOrderItem/{id}")]
        public ResponseViewModel FinishOrderItem(int id)
        {
            var data = db.OrderItem.Find(id);

            if (data.Status == "Cook")
            {
                data.Status = "FinishCook";
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false };
            }
        }
    }
}
