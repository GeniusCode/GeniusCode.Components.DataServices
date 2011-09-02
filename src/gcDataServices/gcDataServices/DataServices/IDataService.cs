using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{
    public interface IScopeAggregate
    {
        IDataScope DataScope { get; set; }
        object Session { get; }
    }

    public class ScopeAggregate : IScopeAggregate
    {
        public IDataScope DataScope { get; set; }
        public object Session { get; set; }
    }


    public interface IDataService
    {
        IScopeAggregate ScopeAggregate { get; }
    }

    public interface IDataService<TScopeAggrageate> : IDataService, IPeerChainDependant<IDataService<TScopeAggrageate>, TScopeAggrageate>
        where TScopeAggrageate : class, IScopeAggregate
    {
    }

}