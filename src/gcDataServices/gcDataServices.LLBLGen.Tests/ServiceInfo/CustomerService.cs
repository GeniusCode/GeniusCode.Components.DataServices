using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeniusCode.Components.DataServices;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests
{
    public class MySession
    {}

    public class MyDataService<T> : DataService<T,MySession> where T : class
    {
    }

    public class CustomerService : MyDataService<CustomerEntity>
    {
        public int GetCustomerCount()
        {
            return GetQuery().Count();
        }  
     
        public OrderService GetOrderService()
        {
            return GetPeerDataService<OrderService>();
        }
    }


    public class OrderService : MyDataService<OrderEntity>
    {
        public int GetOrderCount()
        {
            return GetQuery().Count();
        }
    }

}
