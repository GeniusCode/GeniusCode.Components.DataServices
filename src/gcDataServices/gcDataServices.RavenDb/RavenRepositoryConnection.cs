using System;
using System.Linq;
using Raven.Client;

namespace GeniusCode.Components.DataServices
{
    public class RavenRepositoryConnection : RepositoryConnection
    {
        private IDocumentSession _session;
        private readonly Func<IDocumentSession> _sessionFunc;
        private readonly bool _autoSave;

        public RavenRepositoryConnection(Func<IDocumentSession> sessionFunc, bool autoSave = true)
        {
            _sessionFunc = sessionFunc;
            _autoSave = autoSave;
        }

        protected override void CreateDataConnectionObjects()
        {
            _session = _sessionFunc();
        }

        protected override void PerformApplyPersistContainer(PersistContainer container)
        {
            container.ToDelete.ForEach(a => _session.Delete(a));
            container.ToSave.ForEach(a => _session.Store(a));

            if (_autoSave)
                _session.SaveChanges();
        }

        protected override IQueryable<T> PerformGetQueryFor<T>()
        {
            return _session.Query<T>();
        }
    }
}