using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Name { get; set; }
        public int TableID { get; set; }
        public string OrderServed { get; set; }

    }
}