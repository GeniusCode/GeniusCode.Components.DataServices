using System.Linq;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{

    public class DataService<TScopeAggregate> : PeerBase<IDataService<TScopeAggregate>,TScopeAggregate>,  IDataService<TScopeAggregate>
        where TScopeAggregate : class, IScopeAggregate
    {

        protected TScopeAggregate Scope
        {
            get { return (this as IDependant<TScopeAggregate>).Dependency; }
        }
        
        #region Implementation of IDataService

        IScopeAggregate IDataService.ScopeAggregate
        {
            get { return Scope; }
        }

        #endregion
    }

    public class DataService<T, TScopeAggregate> : DataService<TScopeAggregate>
        where TScopeAggregate : class, IScopeAggregate
        where T : class
    {
        public IQueryable<T> GetQuery()
        {
            return Scope.DataScope.QueryService.GetQueryFor<T>();
        }
    }


}