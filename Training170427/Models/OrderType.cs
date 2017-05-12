using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public List<Models.Order> Order { get; set; }
    }
}