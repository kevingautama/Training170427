using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
    }
}