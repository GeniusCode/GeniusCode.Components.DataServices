using Raven.Client;

namespace GeniusCode.Components.DataServices
{
    public class RavenDataScope : IDataScope
    {

        public RavenDataScope(IDocumentSession session, bool autoSave = true)
        {
            CommandService=  new RavenCommandService(session,autoSave);
            QueryService = new RavenQueryService(session);
        }

        #region Implementation of IDataScope

        public ICommandService CommandService { get; private set; }

        public IQueryService QueryService { get; private set; }

        #endregion
    }
}