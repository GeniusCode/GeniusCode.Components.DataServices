using System.Collections.Generic;
using System.Linq;
using gcDataServices.LLBLGen.Tests.ServiceInfo;
using GeniusCode.Components;
using GeniusCode.Components.DataServices;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using NUnit.Framework;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class LLBLGenTransactionHarnessTests
    {
        [Test]
        public void Should_perform_in_transaction_with_rollback()
        {
            var factories = new List<IFactory<IDataService<IScopeAggregate>>>();
            factories.AddNewDefaultConstructorFactory();
            var abstractFactory = factories.ToDIAbstractFactory<IDataService<IScopeAggregate>, IScopeAggregate>();

            var th = new LLBLGenTransactionHarness<IScopeAggregate>(abstractFactory, () => new DataAccessAdapter(),
                                                                    () => new LinqMetaData());


            //THIS SHOULD HAPPEN IN A TRANSACTION
            th.DoOnDataService<CustomerService>(a =>
                                                    {
                                                        a.SetNameToBrodie("CHOPS");
                                                        Assert.AreEqual("Brody", a.GetQuery().Single(b => b.CustomerId == "CHOPS").CompanyName);
                                                    }, null, false, new ScopeAggregate());


            var adapter = new DataAccessAdapter();
            var md = new LinqMetaData(adapter);


            Assert.AreNotEqual("Brody", md.Customer.Single(b => b.CustomerId == "CHOPS").CompanyName);


        }
    }
}
