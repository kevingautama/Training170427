using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class KitchenViewModel
    {
        public string Status { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
        public List<Models.Order> Order{get;set;}
    }
}