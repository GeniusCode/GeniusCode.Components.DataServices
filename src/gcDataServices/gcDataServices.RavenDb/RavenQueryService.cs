using System.Linq;
using Raven.Client;

namespace GeniusCode.Components.DataServices
{
    public class RavenQueryService : IQueryService
    {
        #region Implementation of IQueryService

        private readonly IDocumentSession _dbSession;

        public RavenPersister(IDocumentSession dbSession)
        {
            _dbSession = dbSession;
        }

        public IQueryable<T> GetQueryFor<T>() where T : class
        {
            return _dbSession.Query<T>();
        }

        #endregion
    }
}