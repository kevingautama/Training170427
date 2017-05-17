using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderItemViewModel
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string Price { get; set; }
        public string Notes { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
        public string TypeName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}