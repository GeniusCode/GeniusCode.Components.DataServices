using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.LinqSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenQueryService : IQueryService
    {
        #region Implementation of IQueryService

        private readonly ILinqMetaData _linqMetaData;

        public LLBLGenQueryService(ILinqMetaData linqMetaData)
        {
            _linqMetaData = linqMetaData;
        }

        public IQueryable<T> GetQueryFor<T>() where T : class
        {
            var name = typeof(T).Name.Replace("Entity", "");
            var pi = _linqMetaData.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var ds = pi.GetValue(_linqMetaData, null);
            return ds as IQueryable<T>;
        }

        #endregion
    }
}