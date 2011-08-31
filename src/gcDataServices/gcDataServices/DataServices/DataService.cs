using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{

 
    public class DataService<T, TSession, TDataScope> : IDataService<T,TSession,TDataScope>
        where T: class
        where TSession : class
        where TDataScope : IDataScope
    {

        protected TDataService GetPeerDataService<TDataService>(object args = null)
            where TDataService :class, IDataService<TSession, TDataScope>
        {
            var asIpc = this as IPeerChainDependant<IDataService<TSession, TDataScope>, Tuple<TSession, TDataScope>>;
            var output =  asIpc.Factory.GetInstance<TDataService>(this,args);
            return output;
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

        IDIAbstractFactory<Tuple<TSession, TDataScope>, IDataService<TSession, TDataScope>> IPeerChainDependant<IDataService<TSession, TDataScope>, Tuple<TSession, TDataScope>>.Factory { get; set; }
    }

    public class DataService<T, TSession> : DataService<T, TSession, IDataScope>
        where T : class
        where TSession : class
    {
        public DataService()
        {
            DataScope = new DataScope();
        }
    }


}