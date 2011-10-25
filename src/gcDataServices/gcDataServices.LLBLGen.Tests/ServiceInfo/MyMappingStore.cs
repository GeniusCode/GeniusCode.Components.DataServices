using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class MyMappingStore : FunctionMappingStore
    {
        public MyMappingStore()
        {
            Add(new FunctionMapping(typeof(Functions), "LengthOfName", 1, "LengthOfName({0})","Northwind","dbo"));
        }
    }
}