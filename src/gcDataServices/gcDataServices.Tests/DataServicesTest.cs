using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using GeniusCode.Components.DataServices;
using NUnit.Framework;

namespace gcDataServices.Tests
{
    [TestFixture]
    public class DataServicesTest
    {
        private readonly Session _session = new Session();
        private List<Person> _dataStore = new List<Person>();
        private IContainer _container;

        public class Session
        {
            public IPrincipal User { get; set; }
        }

        public class Person
        {
            public string Name { internal get; set; }
        }
        

        [SetUp]
        public void Hi()
        {
            _dataStore.Clear();
            _container = CreateContainer();
        }

        [Test]
        public void Should_fetch_items_using_query_service()
        {
            var dataservice = GetService();
            BuildCache();
            var count = dataservice.Query.Count();
            Assert.AreEqual(5, count);
        }

        private void BuildCache()
        {
            _dataStore.Clear();
            _dataStore.Add(new Person { Name = "Joe" });
            _dataStore.Add(new Person { Name = "Bob" });
            _dataStore.Add(new Person { Name = "Frank" });
            _dataStore.Add(new Person { Name = "Bill" });
            _dataStore.Add(new Person { Name = "Thomas" });
        }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ListCommandConnection>().As<LinqRepositoryConnection>();
            builder.RegisterType<FullDataService<Person>>();
            
            Action<List<Person>> dsSetter = items => _dataStore = items.ToList();
            builder.RegisterInstance(dsSetter);

            Func<List<Person>> dsGetter = () => _dataStore;
            builder.RegisterInstance(dsGetter);

            return builder.Build();
        }

        private FullDataService<Person> GetService()
        {
            var q = new NamedParameter("sessionInfo", new object());
            var service = _container.Resolve<FullDataService<Person>>(q);
            return service;
        }

        [Test]
        public void Should_persist_items_using_command_service()
        {
            var dataservice = GetService();
            var p = new Person { Name = "Ryan" };
            dataservice.Save(p);
            Assert.AreEqual(1, _dataStore.Count);
        }

        [Test]
        public void Should_delete_items_using_command_service()
        {
            var dataservice = GetService();
            BuildCache();
            dataservice.Delete(_dataStore.First());
            Assert.AreEqual(4, _dataStore.Count);
        }

        public class ListCommandConnection : LinqRepositoryConnection
        {

            private readonly Action<List<Person>> _dsSetter;
            private readonly Func<List<Person>> _dsGetter;


            public ListCommandConnection(Action<List<Person>> dsSetter, Func<List<Person>> dsGetter)
            {
                _dsSetter = dsSetter;
                _dsGetter = dsGetter;
            }

            protected override void CreateDataConnectionObjects()
            {
            }

            protected override void PerformApplyPersistContainer(PersistContainer container)
            {
                var ds = _dsGetter();

                ds = ds.Union(container.ToSave.OfType<Person>()).ToList();
                ds = ds.Except(container.ToDelete.OfType<Person>()).ToList();

                _dsSetter(ds);
            }

            protected override IQueryable<T> PerformGetQueryFor<T>()
            {
                return _dsGetter().OfType<T>().AsQueryable();
            }
        }

    }


}
