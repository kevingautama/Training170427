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

        [Route("GetAllOrderItem")]
        public List<KitchenViewModel> GetAllOrderItem()
        {
            return service.GetAllOrderItem();
        }
    }
}
