using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderType
    {
        public int OrderTypeID { get; set; }
        public string OrderTypeName { get; set; }
        public List<Order> Order { get; set; }
    }
}