using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class CheckOutModel
    {
        public int OrderID { get; set; }
        public string TableName { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
        public decimal GrandTotal { get; set; }

    }
}