using System.Collections.Generic;
using GeniusCode.Components;
using GeniusCode.Components.DataServices;
using Northwind.DAL.DatabaseSpecific;
using Northwind.DAL.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace gcDataServices.LLBLGen.Tests.Support
{
    public static class TestHelpers
    {
        public static LLBLGenTransactionHarness<IScopeAggregate> GetTestHarness()
        {
            var factories = new List<IFactory<IDataService<IScopeAggregate>>>();
            factories.AddNewDefaultConstructorFactory();
            var abstractFactory = factories.ToDIAbstractFactory<IDataService<IScopeAggregate>, IScopeAggregate>();

            var th = LLBLGenTransactionHarness<IScopeAggregate>.Create<LinqMetaData,FunctionMappingStore>(
                abstractFactory, () => new DataAccessAdapter());

            return th;
        }
    }
}