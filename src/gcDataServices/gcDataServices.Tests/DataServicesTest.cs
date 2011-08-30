using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GeniusCode.Components.DataServices;
using GeniusCode.Components.Factories.DepedencyInjection;
using NUnit.Framework;

namespace gcDataServices.Tests
{
    [TestFixture]
    public class DataServicesTest
    {


        public class Person
        {
            public string Name { get; set; }
        }
        private List<Person> _dataStore = new List<Person>();

        [SetUp]
        public void Hi()
        {
            _dataStore.Clear();
        }

        [Test]
        public void Should_fetch_items_using_query_service()
        {
            var dataservice = GetService();

            BuildCache();

            var count = dataservice.GetQuery().Count();

            Assert.AreEqual(5,count);

        }

        private void BuildCache()
        {
            _dataStore.Add(new Person { Name = "Joe"});
            _dataStore.Add(new Person { Name = "Bob" });
            _dataStore.Add(new Person { Name = "Frank" });
            _dataStore.Add(new Person { Name = "Bill" });
            _dataStore.Add(new Person { Name = "Thomas" });
        }

        private IDataService<Person> GetService()
        {

            var service = new DataService<Person>();
            var asDependant = service as IDependant<Tuple<dynamic, IDataScope>>;
            asDependant.TrySetDependency(new Tuple<dynamic, IDataScope>(new ExpandoObject(), new DataScope
                                                                                 {
                                                                                     QueryService = new ListQueryService(() => _dataStore),
                                                                                     CommandService = new ListCommandService(c => _dataStore = c, () => _dataStore)
                                                                                 }));

            return service;
        }


        [Test]
        public void Should_persist_items_using_command_service()
        {
            var dataservice = GetService();
            var p = new Person {Name = "Ryan"};
            dataservice.DataScope.CommandService.SaveObject(p);
            Assert.AreEqual(1,_dataStore.Count);
        }


        [Test]
        public void Should_delete_items_using_command_service()
        {
            var dataservice = GetService();
            BuildCache();
            
            dataservice.DataScope.CommandService.DeleteObject(_dataStore.First());
            Assert.AreEqual(4, _dataStore.Count);
        }


        internal class ListCommandService : ICommandService
        {
            #region Implementation of ICommandService

            
            private readonly Action<List<Person>> _dsSetter;
            private readonly Func<List<Person>> _dsGetter;


            public ListCommandService(Action<List<Person>> dsSetter, Func<List<Person>> dsGetter)
            {
                _dsSetter = dsSetter;
                _dsGetter = dsGetter;
            }         

            public void ApplyPersistContainer(PersistContainer container)
            {
                var ds = _dsGetter();

                ds = ds.Union(container.ToSave.OfType<Person>()).ToList();
                ds = ds.Except(container.ToDelete.OfType<Person>()).ToList();

                _dsSetter(ds);
            }

            #endregion
        }

        internal class ListQueryService : IQueryService
        {
            private readonly Func<List<Person>> _cache;

            public ListQueryService(Func<List<Person>> cache)
            {
                _cache = cache;
            }

            #region Implementation of IQueryService

            public IQueryable<T> GetQueryFor<T>() where T : class
            {
                    return _cache().OfType<T>().AsQueryable();
            }
            #endregion
        }


    }


}
