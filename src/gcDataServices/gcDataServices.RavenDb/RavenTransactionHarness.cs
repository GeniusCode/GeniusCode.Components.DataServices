using System;
using GeniusCode.Components.DataServices.Transactions;
using GeniusCode.Components.Factories.DepedencyInjection;
using Raven.Client;

namespace GeniusCode.Components.DataServices
{
    public class RavenTransactionHarness<TScopeAggregate> : TransactionHarnessBase<TScopeAggregate> where TScopeAggregate : class, IScopeAggregate
    {
        private Func<IDocumentSession> _getDocumentStoreFunc;

        public RavenTransactionHarness(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractAbstractFactory, Func<IDocumentSession> getDocumentStoreFunc)
            : base(abstractAbstractFactory)
        {
            _getDocumentStoreFunc = getDocumentStoreFunc;
        }

        #region Overrides of TransactionHarnessBase<TScopeAggregate>

        protected override void InitDataPointsOnScope(TScopeAggregate input)
        {
            input.DataScope = new RavenDataScope(_getDocumentStoreFunc(), true);
        }

        #endregion
    }
}