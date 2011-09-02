using System.Linq;
using GeniusCode.Components.DataServices;
using Northwind.DAL.EntityClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{

    public class CustomerService : MyDataService<CustomerEntity>
    {
        public int GetCustomerCount()
        {
            return GetQuery().Count();
        }  
     
        public OrderService GetOrderService()
        {
            return Factory.GetInstance<OrderService>(Scope);
        }


        public void SetNameToBrodie(string custId)
        {
            var entity = GetQuery().Single(a => a.CustomerId == custId);
            entity.CompanyName = "Brody";
            Scope.DataScope.CommandService.SaveObject(entity);
        }


    }
}
