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

        [Route("GetAllOrderItemCateByOrder")]
        public List<KitchenViewModel> GetAllOrderItemCateByOrder()
        {
            return service.GetAllOrderItemCateByOrder();
        }

        [Route("GetAllOrder")]
        public List<Models.Order> GetAllOrder()
        {
            return service.GetAllOrder();
        }

        [Route("GetOrderItemByOrderID/{id}")]
        public List<KitchenViewModel> GetOrderItemByOrderID(int id)
        {
            return service.GetOrderItemByOrderID(id);
        }

        [Route("GetOrderItemPrint/{id}")]
        public Models.Order GetOrderItemPrint(int id)
        {
            Models.Order data = new Models.Order();
            var order = db.Order.Find(id);
            data.Name = order.Name;
            data.OrderID = order.OrderID;
            data.TableName = (from a in db.Track
                             where a.OrderID == order.OrderID
                             select a.Table.TableName).FirstOrDefault();
            var orderitem = (from a in db.OrderItem
                             where a.IsDeleted != true && a.Status == "Cook" && a.OrderID == order.OrderID
                             select new OrderItemViewModel
                             {
                                 MenuName = a.Menu.MenuName,
                                 Qty = a.Qty,
                                 Notes = a.Notes
                             }).ToList();
            data.OrderItem = orderitem;
            return data;
        }
    }
}
