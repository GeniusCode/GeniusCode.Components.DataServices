using System;
using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public class DataService<T> : DataService, IRepository<T> where T : class
    {

        protected new internal LinqRepositoryConnection RepositoryConnection
        {
            get { return base.RepositoryConnection as LinqRepositoryConnection; }
        }

        public DataService(LinqRepositoryConnection repositoryConnection) : base(repositoryConnection)
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
        public DataService(RepositoryConnection repositoryConnection)
        {
            RepositoryConnection = repositoryConnection;
        }

        protected internal RepositoryConnection RepositoryConnection { get; private set; }

    }

}