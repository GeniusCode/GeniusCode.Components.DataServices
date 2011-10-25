using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using GeniusCode.Components.DataServices.Transactions;
using Northwind.DAL.EntityClasses;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using gcDataServices.LLBLGen.Tests.ServiceInfo;
using GeniusCode.Components.DataServices;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using NUnit.Framework;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class LLBLGenTransactionFacadeTests
    {
        private readonly TransactionFacade<Session> _facade = new TransactionFacade<Session>();
        private IContainer _container;
        
        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            Func<IDataAccessAdapter> adapterFunc = ()=> new DataAccessAdapter();
            builder.RegisterInstance(adapterFunc);
            builder.RegisterType<UnitOfWork2>();
            builder.RegisterType<LinqMetaData>().As<ILinqMetaData>();
            builder.RegisterType<MyMappingStore>().As<FunctionMappingStore>();
            builder.RegisterType<LLBLGenRepositoryConnection>().As<RepositoryConnection>();
            builder.RegisterType<CustomerService>();
            builder.Register(c => new Session());

            _container = builder.Build();
        }
            
        

        [Test]
        public void Should_perform_in_transaction_with_rollback()
        {
            var service = _container.Resolve<CustomerService>();
           
            //THIS SHOULD HAPPEN IN A TRANSACTION
            _facade.PerformOnDataService(service, a =>
                                                    {
                                                        a.SetNameToBrodie("CHOPS");
                                                        Assert.AreEqual("Brody", a.Query.Single(b => b.CustomerId == "CHOPS").CompanyName);
                                                    },false);

            // transaction should have rolled back - iow, there should not be any more brody
            var adapter = new DataAccessAdapter();
            var md = new LinqMetaData(adapter);
            
            Assert.AreNotEqual("Brody", md.Customer.Single(b => b.CustomerId == "CHOPS").CompanyName);
        }

        [Test]
        public void Should_call_function()
        {
            List<int> items = null;
            var connection =_container.Resolve<RepositoryConnection>();
            _facade.Perform(()=>
                             {
                                 items = (from t in connection.GetQueryFor<CustomerEntity>()
                                          select Functions.LengthOfName(t.CustomerId)).ToList();
                             },false,connection);
           Assert.IsTrue(items.Any());
        }


    }
}
