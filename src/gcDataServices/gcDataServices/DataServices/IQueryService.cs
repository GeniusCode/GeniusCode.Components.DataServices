using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public interface IQueryService
    {
        IQueryable<T> GetQueryFor<T>() where T : class;
    }
}