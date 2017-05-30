﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training170427.Models
{
    public class AddOrder
    {
        public int OrderID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int Status { get; set; }
        public int? TableID { get; set; }
        public string TableName { get; set; }
        public List<CategoryViewModel> Category {get;set;}
        public List<Table> Table { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
    }
}