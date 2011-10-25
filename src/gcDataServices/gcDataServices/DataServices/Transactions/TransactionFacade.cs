using System;
using System.Security.Principal;
using System.Transactions;

namespace GeniusCode.Components.DataServices.Transactions
{
    public interface ITransactionFacade
    {
        void PerformOnDataService<T>(T instance, Action<T> toDo, bool close)
            where T : DataService;


    }

    public class TransactionFacade : ITransactionFacade
    {


        public void PerformOnDataService<T>(T instance, Action<T> toDo, bool close)
            where T : DataService
        {
            Perform(() => toDo(instance) , close, instance.RepositoryConnection);
        }

        public void Perform(Action action, bool close, RepositoryConnection repositoryConnection)
        {
            using (var t = new TransactionScope())
            {
                repositoryConnection.CreateDataConnectionObjects();
                action();

                if (close)
                    t.Complete();
            }

        }

    }
}
