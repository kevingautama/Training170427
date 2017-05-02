using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class OrderItemViewModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string Price { get; set; }
        public int Qty { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}