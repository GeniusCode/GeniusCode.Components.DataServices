using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Query { get; }
        void Save(T item);
        void Delete(T item);
    }
}