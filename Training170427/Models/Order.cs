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
        public int TypeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string TableName { get; set; }
        public string OrderServed { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }


    }
}