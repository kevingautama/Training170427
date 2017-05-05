using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
        public List<CategoryViewModel> Category { get; set; }
    }
}