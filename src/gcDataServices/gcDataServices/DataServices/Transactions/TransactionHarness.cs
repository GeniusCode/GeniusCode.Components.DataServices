using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using GeniusCode.Components.Factories.DepedencyInjection;
using GeniusCode.Components;

namespace GeniusCode.Components.DataServices.Transactions
{
    public abstract class TransactionHarnessBase<TScopeAggregate>
        where TScopeAggregate :class, IScopeAggregate
    {
        private readonly IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> _abstractFactory;

        protected TransactionHarnessBase(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractAbstractFactory)
        {
            _abstractFactory = abstractAbstractFactory;
        }

        protected abstract void InitDataPointsOnScope(TScopeAggregate input);

        public void DoOnDataService<T>(Action<T> toDo, object args, bool close, TScopeAggregate scope)
            where T: class, IDataService<TScopeAggregate>
        {

            using (var t = new TransactionScope())
            {
                InitDataPointsOnScope(scope);
                var factory = _abstractFactory.GetInstance<T>(scope, args);        

                toDo(factory);

                if (close)
                    t.Complete();
            }
        }


    }
}
