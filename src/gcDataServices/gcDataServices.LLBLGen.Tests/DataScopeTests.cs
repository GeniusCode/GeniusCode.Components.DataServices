using System.Linq;
using GeniusCode.Components.DataServices;
using NUnit.Framework;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.EntityClasses;
using Northwind.DAL.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace gcDataServices.LLBLGen.Tests
{
    [TestFixture]
    public class DataScopeTests
    {
        public class Functions
        {
            public static int LengthOfName(string customerId)
            {
                return 0;
            }
        }

        public class MappingStore : FunctionMappingStore
        {
            public MappingStore()
            {
                Add(new FunctionMapping(typeof(Functions), "LengthOfName", 1, "LengthOfName({0})","Northwind","dbo"));
            }
        }

        [Test]
        public void Should_inject_mapping_store_into_datascope()
        {
            var hi = LLBLGenDataScope.Create<LinqMetaData,MappingStore>(new DataAccessAdapter());

            var q = from t in hi.QueryService.GetQueryFor<CustomerEntity>()
                    select Functions.LengthOfName(t.CustomerId);

            var items = q.ToList();

            Assert.IsTrue(items.Any());
        }
    }
}