using GeniusCode.Components.DataServices;

namespace gcDataServices.LLBLGen.Tests.ServiceInfo
{
    public class MyDataService<T> : DataService<T,MySession> where T : class
    {
    }
}