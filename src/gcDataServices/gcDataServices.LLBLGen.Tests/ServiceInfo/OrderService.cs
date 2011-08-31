using System.Linq;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class OrderService : MyDataService<OrderEntity>
    {
        public int GetOrderCount()
        {
            return GetQuery().Count();
        }
    }
}