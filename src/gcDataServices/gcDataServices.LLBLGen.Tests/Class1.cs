using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeniusCode.Components;
using GeniusCode.Components.DataServices;
using GeniusCode.Components.Factories.DepedencyInjection;
using NUnit.Framework;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class Class1 
    {

        [Test]
        public void Should_connect()
        {
            var af = GetSimpleDIAbstractFactory();
            var scope = GetSimpleScope();

            var instance = af.GetInstance<CustomerService>(scope);
            Assert.IsTrue(instance.GetCustomerCount() > 5,"Could not resolve Customer Service");
        }

        [Test]
        public void Should_connect_using_peer_chain()
        {
            var af = GetSimpleDIAbstractFactory();
            var scope = GetSimpleScope();

            var instance = af.GetInstance<CustomerService>(scope);
            var instance2 = instance.GetOrderService();
            Assert.IsTrue(instance2.GetOrderCount() > 5, "Could not resolve Orders Service");
        }

        private static DIAbstractFactory<Tuple<MySession, IDataScope>, IDataService<MySession, IDataScope>> GetSimpleDIAbstractFactory()
        {
            var factories = new List<IFactory<IDataService<MySession, IDataScope>>>();
            factories.AddNewDefaultConstructorFactory();

            var af =
                factories.ToDIAbstractFactory<IDataService<MySession, IDataScope>, Tuple<MySession, IDataScope>>();
            return af;
        }

        private static Tuple<MySession, IDataScope> GetSimpleScope()
        {
            var adapter = new DataAccessAdapter();
            var metaData = new LinqMetaData(adapter);
            var scope = new LLBLGenDataScope(adapter, metaData);
            var dep = new Tuple<MySession, IDataScope>(new MySession(), scope);
            return dep;
        }
    }
}
