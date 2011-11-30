using System;
using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
    public class LLBLGenRepositoryConnection : LinqRepositoryConnection
    {
        #region Implementation of IQueryService

        private readonly Func<IDataAccessAdapter> _adapterFunc;
        private readonly ILinqMetaData _linqMetaData;
        private readonly FunctionMappingStore _mappingStore;

        #endregion

        protected override void CreateDataConnectionObjects()
        {
            _adapter = _adapterFunc();
            InitLinqMetadata(_adapter, _linqMetaData, _mappingStore);
        }

        protected override void PerformApplyPersistContainer(PersistContainer container)
        {
            container.ToDelete.OfType<IEntity2>().ToList().ForEach(a => _uow.AddForDelete(a));
            container.ToSave.OfType<IEntity2>().ToList().ForEach(a => _uow.AddForSave(a, _refetch));

            if (_autoSave)
                _uow.Commit(_adapter);

        }

        protected override IQueryable<T> PerformGetQueryFor<T>()
        {
            var name = typeof(T).Name.Replace("Entity", "");
            var pi = _linqMetaData.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var ds = pi.GetValue(_linqMetaData, null);
            return ds as IQueryable<T>;
        }

        #region Implementation of ICommandService

        private IDataAccessAdapter _adapter;
        private readonly UnitOfWork2 _uow;
        private readonly bool _autoSave;
        private readonly bool _refetch;

        public LLBLGenRepositoryConnection(
            Func<IDataAccessAdapter> adapterFunc, 
            UnitOfWork2 uow, 
            ILinqMetaData linqMetaData, 
            FunctionMappingStore mappingStore, 
            bool refetch = true, 
            bool autoSave = true)
        {
            _adapterFunc = adapterFunc;
            _linqMetaData = linqMetaData;
            _mappingStore = mappingStore;
            _refetch = refetch;
            _autoSave = autoSave;
            _uow = uow;
        }

        public IDataAccessAdapter Adapter
        {
            get { return _adapter; }
        }

        public UnitOfWork2 UnitOfWork
        {
            get { return _uow; }
        }



        #endregion

        /// <summary>
        /// Initialize the LinqMetadata object with an adapter instance, as well as a mapping store
        /// </summary>
        /// <param name="adapter">adapter instance to use</param>
        /// <param name="metaData">existing metadata object</param>
        /// <param name="mappingStore">mappingstore object</param>
        private static void InitLinqMetadata(IDataAccessAdapter adapter, ILinqMetaData metaData, FunctionMappingStore mappingStore)
        {
            ReflectionHelper.SetPropertyValue("AdapterToUse", metaData, adapter);

            if (mappingStore != null)
                ReflectionHelper.SetPropertyValue("CustomFunctionMappings", metaData, mappingStore);
        }

    }
}