using System;
using System.Collections.Generic;
using System.Text;
using GeniusCode.Components.DataServices.Transactions;
using Raven.Client;

namespace GeniusCode.Components.DataServices
{

    public class RavenTransactionHarness<TScopeAggreagate> : TransactionHarnessBase<TScopeAggreagate> where TScopeAggreagate : class
    {
        public RavenTransactionHarness(IDIAbstractFactory<TScopeAggregate, IDataService<TScopeAggregate>> abstractAbstractFactory) : base(abstractAbstractFactory)
        {
        }
    }

    public class RavenCommandService : ICommandService
    {
        #region Implementation of ICommandService

        private readonly IDocumentSession _documentSession;
        private readonly bool _autoSave;

        public RavenCommandService(IDocumentSession documentSession, bool autoSave = true)
        {
            _documentSession = documentSession;
            _autoSave = autoSave;
        }

        public void ApplyPersistContainer(PersistContainer container)
        {           
            container.ToDelete.ForEach(a=> _documentSession.Delete(a));
            container.ToSave.ForEach(a=> _documentSession.Store(a));

            if (_autoSave)
                _documentSession.SaveChanges();
        }

        #endregion
    }

}
