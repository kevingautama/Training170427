﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Training170427.Models;

namespace Training170427.Service
{
    public class OrderService
    {
        private RestaurantEntities db = new RestaurantEntities();
        public List<Models.Order> GetOrder()
        {
            List<Models.Order> listdata = new List<Models.Order>();
            var order = (from a in db.Order
                         where a.Finish == false && a.IsDeleted == false
                         select a).ToList();
            foreach (var item in order)
            {
                Models.Order data = new Models.Order();
                if (db.Type.Find(item.TypeID).TypeName == "Order")
                {
                    var table = (from a in db.Track
                                 where a.OrderID == item.OrderID
                                 select a).FirstOrDefault();
                    data.Name = table.Table.TableName;
                    data.TableID = table.TableID;
                }

                if (db.Type.Find(item.TypeID).TypeName == "TakeAway")
                {
                    data.Name = "Admin";
                }
                data.OrderID = item.OrderID;

                var finish = (from a in db.OrderItem
                              where a.OrderID == item.OrderID && a.IsDeleted != false
                              select a).ToList();
                var i = 0;
                foreach (var item2 in finish)
                {
                    if (item2.Status == "FinishCook")
                    {
                        i++;
                    }
                }
                data.OrderServed = i.ToString() + "/" + finish.Count().ToString();

                listdata.Add(data);
            }
            return listdata;
        }
    }
}