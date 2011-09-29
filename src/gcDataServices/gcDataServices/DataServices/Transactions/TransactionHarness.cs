using System;
using System.Transactions;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices.Transactions
{

    public abstract class TransactionHarnessBase<TScopeAggregate> 
        where TScopeAggregate : class, IScopeAggregate
    {
        private readonly IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> _abstractFactory;

        protected TransactionHarnessBase(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractAbstractFactory)
        {
            _abstractFactory = abstractAbstractFactory;
        }

        protected abstract void InitDataPointsOnScope(TScopeAggregate input);

        public void DoOnDataService<T>(Action<T> toDo, object args, bool close, TScopeAggregate scope)
            where T : class, IDataService<TScopeAggregate>
        {
            var factory = _abstractFactory.GetInstance<T>(scope, args);
            DoOnScope(a => toDo(factory), close, scope);
        }

        public void DoOnScope(Action<TScopeAggregate> scopeFunction, bool close, TScopeAggregate scopeAggregate)
        {
            using (var t = new TransactionScope())
            {
                InitDataPointsOnScope(scopeAggregate);
                scopeFunction(scopeAggregate);
                InitDataPointsOnScope(scopeAggregate);

                if (close)
                    t.Complete();
            }

        }

    }
}
