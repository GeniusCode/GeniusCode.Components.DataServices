using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace GeniusCode.Components.DataServices
{
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

        public IDataAccessAdapter Adapter
        {
            get { return _adapter; }
        }

        public UnitOfWork2 UnitOfWork
        {
            get { return _uow; }
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
