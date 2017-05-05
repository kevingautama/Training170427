using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
        public List<CategoryViewModel> Category { get; set; }
    }
}