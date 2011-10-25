using System;
using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public class DataService<T, TSessionInfo> : DataService<TSessionInfo>, IRepository<T> where T : class
    {
        public DataService(RepositoryConnection repositoryConnection, TSessionInfo sessionInfo) : base(repositoryConnection, sessionInfo)
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


    public class DataService<TSessionInfo>
    {
        public DataService(RepositoryConnection repositoryConnection, TSessionInfo sessionInfo)
        {
            RepositoryConnection = repositoryConnection;
            SessionInfo = sessionInfo;
        }

        protected internal RepositoryConnection RepositoryConnection { get; private set; }
        protected internal TSessionInfo SessionInfo { get; private set; }

        internal void CloneFromPeer(DataService<TSessionInfo> existingPeer)
        {
            RepositoryConnection = existingPeer.RepositoryConnection;
            SessionInfo = existingPeer.SessionInfo;
        }

        protected void DoOnPeer<TPeer> (TPeer instance, Action<TPeer> toDo) 
            where TPeer : DataService<TSessionInfo>
        {
            instance.CloneFromPeer(this);
            toDo(instance);
        }

    }

}