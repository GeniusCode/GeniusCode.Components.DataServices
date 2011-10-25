using System.Linq;
using GeniusCode.Components.DataServices;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class OrderService : DataService<OrderEntity,Session>
    {
        public OrderService(RepositoryConnection repositoryConnection, Session sessionInfo) : base(repositoryConnection, sessionInfo)
        {
        }

        public int GetOrderCount()
        {
            return Query.Count();
        }
    }
}