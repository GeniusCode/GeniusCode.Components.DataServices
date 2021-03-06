using System.Linq;
using GeniusCode.Components.DataServices;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class OrderService : FullDataService<OrderEntity>
    {
        public OrderService(LinqRepositoryConnection repositoryConnection) : base(repositoryConnection)
        {
        }

        public int GetOrderCount()
        {
            return Query.Count();
        }
    }
}