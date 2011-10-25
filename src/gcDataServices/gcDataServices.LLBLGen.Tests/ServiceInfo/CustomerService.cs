using System.Linq;
using GeniusCode.Components.DataServices;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class CustomerService : DataService<CustomerEntity,Session>
    {
        public CustomerService(RepositoryConnection repositoryConnection, Session sessionInfo) : base(repositoryConnection, sessionInfo)
        {
        }

        public int GetCustomerCount()
        {
            return Query.Count();
        }  
     
        public OrderService GetOrderService()
        {
            return new OrderService(RepositoryConnection, SessionInfo);
        }


        public void SetNameToBrodie(string custId)
        {
            var entity = Query.Single(a => a.CustomerId == custId);
            entity.CompanyName = "Brody";
            Save(entity);
        }


    }
}
