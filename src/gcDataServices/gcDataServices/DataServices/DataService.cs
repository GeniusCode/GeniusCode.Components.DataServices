using System;
using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public class DataService<T> : DataService, IRepository<T> where T : class
    {
        public DataService(RepositoryConnection repositoryConnection, object sessionInfo) : base(repositoryConnection, sessionInfo)
        {
        }

        public IQueryable<T> Query
        {
            get { return RepositoryConnection.GetQueryFor<T>();
            }
        }

        public void Save(T item)
        {
            RepositoryConnection.SaveObject(item);
        }

        public void Delete(T item)
        {
            RepositoryConnection.DeleteObject(item);
        }
    }

    public class DataService
    {
        public DataService(RepositoryConnection repositoryConnection, object sessionInfo)
        {
            RepositoryConnection = repositoryConnection;
            SessionInfo = sessionInfo;
        }

        protected internal RepositoryConnection RepositoryConnection { get; private set; }
        protected internal object SessionInfo { get; private set; }

        internal void CloneFromPeer(DataService existingPeer)
        {
            RepositoryConnection = existingPeer.RepositoryConnection;
            SessionInfo = existingPeer.SessionInfo;
        }

        protected void DoOnPeer<TPeer> (TPeer instance, Action<TPeer> toDo) 
            where TPeer : DataService
        {
            instance.CloneFromPeer(this);
            toDo(instance);
        }

    }

}