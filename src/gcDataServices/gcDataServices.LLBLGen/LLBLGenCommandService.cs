using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenDataScope : IDataScope
    {
        private readonly IDataAccessAdapter _adapter;
        private readonly ILinqMetaData _metaData;
        private readonly UnitOfWork2 _unitOfWork2;

        public LLBLGenDataScope(IDataAccessAdapter adapter, ILinqMetaData metaData)
        {
            _adapter = adapter;
            _metaData = metaData;


            QueryService = new LLBLGenQueryService(_metaData);

            _unitOfWork2 = new UnitOfWork2();
            CommandService = new LLBLGenCommandService(_unitOfWork2, _adapter);
        }


        public LLBLGenQueryService QueryService { get; private set; }
        public LLBLGenCommandService CommandService { get; private set; }

        ICommandService IDataScope.CommandService
        {
            get { return CommandService; }
        }

        IQueryService IDataScope.QueryService
        {
            get { return QueryService; }
        }
    }

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

    public class LLBLGenCommandService : ICommandService
    {
        #region Implementation of ICommandService

        private readonly IDataAccessAdapter _adapter;
        private readonly UnitOfWork2 _uow;
        private readonly bool _autoSave;
        private readonly bool _refetch;

        public LLBLGenCommandService(UnitOfWork2 uow, IDataAccessAdapter adapter, bool refetch = true, bool autoSave = true)
        {
            _refetch = refetch;
            _autoSave = autoSave;
            _uow = uow;
            _adapter = adapter;
        }

        public void ApplyPersistContainer(PersistContainer container)
        {
            container.ToDelete.OfType<IEntity2>().ToList().ForEach(a => _uow.AddForDelete(a));
            container.ToSave.OfType<IEntity2>().ToList().ForEach(a => _uow.AddForSave(a, _refetch));

            if (_autoSave)
                _uow.Commit(_adapter);

        }

        #endregion
    }
}
