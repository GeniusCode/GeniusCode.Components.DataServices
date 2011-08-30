using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public interface IDataService
    {
        IDataScope DataScope { get; set; }
        object Session { get; set; }
    }

    public interface IDataService<out T> : IDataService
    {
        IQueryable<T> GetQuery();
    }
}