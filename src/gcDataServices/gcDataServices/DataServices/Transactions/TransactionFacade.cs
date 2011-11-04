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


    public class Performer<TDataService>  where TDataService : DataService
    {
        private readonly TransactionFacade _facade;
        private readonly TDataService _service;

        public Performer(TransactionFacade facade, TDataService service)
        {
            _facade = facade;
            _service = service;
        }

        public void Invoke(Action<TDataService> toInvoke, bool close = true)
        {
            (_facade as ITransactionFacade).PerformOnDataService(_service,toInvoke,close);
        }

        public T Select<T>(Func<TDataService, T> toSelect, bool close = false)
        {
            var output = default(T);
            (_facade as ITransactionFacade).PerformOnDataService(_service,s=> output = toSelect(s), close);
            return output;
        }
    }

    public class TransactionFacade : ITransactionFacade
    {
        public Performer<TDataService> On<TDataService>(TDataService service, bool close = true) where TDataService : DataService
        {
            return new Performer<TDataService>(this, service);
        }


        void ITransactionFacade.PerformOnDataService<T>(T instance, Action<T> toDo, bool close)
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
