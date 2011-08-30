using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public interface IDataService
    {
        IDataScope DataScope { get;  }
        object Session { get;  }
    }

    public interface IDataService<out T> : IDataService
    {
        IQueryable<T> GetQuery();
    }
}