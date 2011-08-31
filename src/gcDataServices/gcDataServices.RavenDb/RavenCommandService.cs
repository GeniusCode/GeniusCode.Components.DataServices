using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client;

namespace GeniusCode.Components.DataServices
{
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
