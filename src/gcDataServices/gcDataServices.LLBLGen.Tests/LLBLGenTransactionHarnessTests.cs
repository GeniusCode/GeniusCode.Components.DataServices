using System.Collections.Generic;
using System.Linq;
using Northwind.DAL.EntityClasses;
using gcDataServices.LLBLGen.Tests.ServiceInfo;
using GeniusCode.Components.DataServices;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using NUnit.Framework;
using gcDataServices.LLBLGen.Tests.Support;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class LLBLGenTransactionHarnessTests
    {
        [Test]
        public void Should_perform_in_transaction_with_rollback()
        {
            var th = TestHelpers.GetTestHarness();
            //THIS SHOULD HAPPEN IN A TRANSACTION
            th.DoOnDataService<CustomerService>(a =>
                                                    {
                                                        a.SetNameToBrodie("CHOPS");
                                                        Assert.AreEqual("Brody", a.GetQuery().Single(b => b.CustomerId == "CHOPS").CompanyName);
                                                    }, null, false, new ScopeAggregate());

            // transaction should have rolled back - iow, there should not be any more brody
            var adapter = new DataAccessAdapter();
            var md = new LinqMetaData(adapter);
            
            Assert.AreNotEqual("Brody", md.Customer.Single(b => b.CustomerId == "CHOPS").CompanyName);
        }

        [Test]
        public void Should_call_function()
        {
            var th = TestHelpers.GetTestHarness();

            List<int> items = null;

            th.DoOnScope(s=>
                             {
                                 items = (from t in s.DataScope.QueryService.GetQueryFor<CustomerEntity>()
                                         select DataScopeTests.Functions.LengthOfName(t.CustomerId)).ToList();

                             },false,new ScopeAggregate());

           Assert.IsTrue(items.Any());

        }


    }
}
