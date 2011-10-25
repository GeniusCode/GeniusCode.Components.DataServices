using System.Collections.Generic;
using System.Data;
using System.Linq;
using gcDataServices.LLBLGen.Tests.ServiceInfo;
using GeniusCode.Components;
using GeniusCode.Components.DataServices;
using GeniusCode.Components.Factories.DepedencyInjection;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using NUnit.Framework;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class BasicTests
    {

        [Test]
        public void Should_connect()
        {
            IDataAccessAdapter adapter;
            var af = GetSimpleDIAbstractFactory();
            var scope = GetSimpleScope(out adapter);

            var instance = af.GetInstance<CustomerService>(scope);
            Assert.IsTrue(instance.GetCustomerCount() > 5, "Could not resolve Customer Service");
        }

        [Test]
        public void Should_connect_using_peer_chain()
        {
            IDataAccessAdapter adapter;
            var af = GetSimpleDIAbstractFactory();
            var scope = GetSimpleScope(out adapter);

            var instance = af.GetInstance<CustomerService>(scope);
            var instance2 = instance.GetOrderService();
            Assert.IsTrue(instance2.GetOrderCount() > 5, "Could not resolve Orders Service");
        }


        [Test]
        public void Should_update_in_transaction()
        {
            var af = GetSimpleDIAbstractFactory();
            IDataAccessAdapter adapter;
            var scope = GetSimpleScope(out adapter);

            var service = af.GetInstance<CustomerService>(scope);

            adapter.StartTransaction(IsolationLevel.Unspecified, "test");

            service.SetNameToBrodie("CHOPS");
            var count = service.GetQuery().Where(a => a.CompanyName == "Brody").Count();
            adapter.Rollback();

            Assert.AreEqual(1, count);

        }

        private static DIAbstractFactory<IScopeAggregate, IDataService<IScopeAggregate>> GetSimpleDIAbstractFactory()
        {
            var factories = new List<IFactory<IDataService<IScopeAggregate>>>();
            factories.AddNewDefaultConstructorFactory();

            var af = factories.ToDIAbstractFactory<IDataService<IScopeAggregate>, IScopeAggregate>();
            return af;
        }

        private static ScopeAggregate GetSimpleScope(out IDataAccessAdapter adapter)
        {
            adapter = new DataAccessAdapter();
            var scope = LLBLGenDataScope.Create<LinqMetaData, FunctionMappingStore>(adapter);
            var dep = new ScopeAggregate { DataScope = scope, Session = new object() };
            return dep;
        }
    }
}
