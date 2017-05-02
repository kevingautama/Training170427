using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class AddOrder
    {
        public int TypeID { get; set; }
        public int TableID { get; set; }
        public List<OrderItem> OrderItem { get; set; }
        public List<Table> Table { get; set; }
    }
}