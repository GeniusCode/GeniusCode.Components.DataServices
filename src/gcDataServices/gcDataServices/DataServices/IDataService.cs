using System;
using System.Linq;
using GeniusCode.Components.Factories.DepedencyInjection;

namespace GeniusCode.Components.DataServices
{
    public interface IDataService
    {
        IDataScope DataScope { get; }
        object Session { get; }
    }

    public interface IDataService<TSession, TDataScope> : IPeerChainDependant<IDataService<TSession,TDataScope>,  Tuple<TSession, TDataScope>>
    {
    }

    public interface IDataService<out T> : IDataService
    {
        IQueryable<T> GetQuery();
    }

    public interface IDataService<out T, TSession, TDataScope> : IDataService<T>, IDataService<TSession, TDataScope>
        where T : class
    {
    }

}