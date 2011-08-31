using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{
    public class DataService<T, TSession, TDataScope> : IDataService<T>, IDependant<Tuple<TSession, TDataScope>>
        where T: class, IDependant<Tuple<TSession,TDataScope>>
        where TSession : class
        where TDataScope : IDataScope
    {

        private DIAbstractFactory<Tuple<TSession, TDataScope>, T> _factory;

        protected TDataService GetPeerDataService<TDataService>()
            where TDataService :class, T, IDependant<Tuple<TSession,TDataScope>>
        {
            _factory.

        }

        #region Implementation of IDataService

        public IQueryable<T> GetQuery()
        {
            return DataScope.QueryService.GetQueryFor<T>();
        }

        public TSession Session { get; internal set; }
        public TDataScope DataScope { get; internal set; }
        
        #endregion

        IDataScope IDataService.DataScope
        {
            get { return DataScope; }
        }

        object IDataService.Session
        {
            get { return Session; }
        }

        #region Implementation of IDependant<Tuple<TSession,TDataScope>>

        bool IDependant<Tuple<TSession, TDataScope>>.TrySetDependency(Tuple<TSession, TDataScope> args)
        {
            Session = args.Item1;
            DataScope = args.Item2;
            return true;
        }

        Tuple<TSession, TDataScope> IDependant<Tuple<TSession, TDataScope>>.Dependency
        {
            get { return new Tuple<TSession, TDataScope>(Session,DataScope); }
        }

        #endregion
    }

    public class DataService<T, TSession> : DataService<T, TSession, IDataScope> where T : class where TSession : class 
    {
       public DataService()
       {
           DataScope = new DataScope();
       }      
    }

    public class DataScope : IDataScope
   {
       public ICommandService CommandService { get; set; }
       public IQueryService QueryService { get; set; }
   }

    public class DataService<T> : DataService<T, dynamic> where T: class
    {
        public DataService()
        {
            Session = new ExpandoObject();
        }
    }


    public class Extensions
    {
        public static DIAbstractFactory<Tuple<TSession,TDataScope>,T> GetDataServiceFactory<T,TSession,TDataScope>()
            where T: class 
            where TSession : class
            where TDataScope : DataScope
        {
            
        }
    }

    public class DataServiceFactory<T,TSession,TDataScope> : DIAbstractFactory<Tuple<TSession,TDataScope>,T>
        where T: class
    {
        public DataServiceFactory(IEnumerable<IFactory<T>> providers) : base(providers)
        {
        }
    }

}