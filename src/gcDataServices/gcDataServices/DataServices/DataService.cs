using System;
using System.Dynamic;
using System.Linq;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{
    public class DataService<T, TSession, TDataScope> : IDataService<T>, IDependant<Tuple<TSession, TDataScope>>
        where T: class
        where TSession : class
        where TDataScope : IDataScope
    {
        #region Implementation of IDataService

        public IQueryable<T> GetQuery()
        {
            return DataScope.QueryService.GetQueryFor<T>();
        }

        public TSession Session { get; set; }
        public TDataScope DataScope { get; set; }
        
        #endregion

        IDataScope IDataService.DataScope
        {
            get { return DataScope; }
            set { DataScope = (TDataScope)value; }
        }

        object IDataService.Session
        {
            get { return Session; }
            set { Session = (TSession) value; }
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
           DataScope = new DataScopeImpl();
       }

       internal class DataScopeImpl : IDataScope
       {
           public ICommandService CommandService { get; set; }
           public IQueryService QueryService { get; set; }
      }
    }

    public class DataService<T> : DataService<T, dynamic> where T: class
    {
        public DataService()
        {
            Session = new ExpandoObject();
        }
    }



}