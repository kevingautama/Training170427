using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Training170427;
using Training170427.Models;
using Training170427.Service;

namespace Training170427.Controllers
{
    [RoutePrefix("api/WaiterAPI")]
    public class WaiterAPIController : ApiController
    {
        private RestaurantEntities db = new RestaurantEntities();
        OrderService service = new OrderService();

        // GET: api/WaiterAPI
        public List<OrderType> GetOrder()
        {
            var data = service.GetOrder();
            return data;

        }

        [HttpGet]
        [Route("DetailOrder/{id}")]
        public Models.Order DetailOrder(int id)
        {
            var data = service.DetailOrder(id);
            return data;
        }

        //[HttpGet]
        //[Route("ChooseMenu/{id}")]
        //public Models.AddOrder ChooseMenu(int? TableID)
        //{
        //    var data = service.ChooseMenu(TableID);
        //    return data;
        //}

        [HttpGet]
        [Route("Table")]
        public List<Models.TableViewModel> GetTable()
        {
            var data = service.Table();
            return data;
        }

        [Route("ServedOrder/{id}")]
        public ResponseViewModel ServedOrder(int id)
        {
            var orderitem = db.OrderItem.Find(id);

            if (orderitem.IsDeleted != true)
            {
                orderitem.Status = "Served";
                db.Entry(orderitem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false }; ;
            }
        }
        [Route("CancelOrder/{id}")]
        public ResponseViewModel CancelOrder(int id)
        {
            var orderitem = db.OrderItem.Find(id);

            if (orderitem.IsDeleted != true)
            {
                orderitem.Status = "Cancel";
                orderitem.IsDeleted = true;
                db.Entry(orderitem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new ResponseViewModel { Status = true };
            }
            else
            {
                return new ResponseViewModel { Status = false }; ;
            }
        }

        [Route("PayOrder/{id}")]
        public ResponseViewModel PayOrder(int id)
        {
            return service.Pay(id);
        }

        [Route("Category/{id}")]
        public Models.AddOrder Category(int TypeID, int? TableID)
        {
            var data = service.Category(TypeID, TableID);
            return data;
        }


        // GET: api/WaiterAPI/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/WaiterAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WaiterAPI
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Order.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/WaiterAPI/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Order.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Order.Count(e => e.OrderID == id) > 0;
        }
    }
}