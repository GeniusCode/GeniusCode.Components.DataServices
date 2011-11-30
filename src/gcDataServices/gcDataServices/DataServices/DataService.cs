using System;
using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public abstract class DataService
    {
        protected DataService(RepositoryConnection repositoryConnection)
        {
            RepositoryConnection = repositoryConnection;
        }

        protected internal RepositoryConnection RepositoryConnection { get; private set; }

    }

}