using System.Linq;

namespace GeniusCode.Components.DataServices
{
    public abstract class LinqRepositoryConnection : RepositoryConnection
    {
        protected abstract IQueryable<T> PerformGetQueryFor<T>() where T : class;

        public IQueryable<T> GetQueryFor<T>() where T : class
        {
            return PerformGetQueryFor<T>();
        }

    }

    public abstract class RepositoryConnection
    {
        public void ApplyPersistContainer(PersistContainer container)
        {
            PerformApplyPersistContainer(container);
        }



        public void SaveObject(object toSave)
        {
            var pc = new PersistContainer();
            pc.ToSave.Add(toSave);
            ApplyPersistContainer(pc);
        }

        public void DeleteObject(object toDelete)
        {
            var pc = new PersistContainer();
            pc.ToDelete.Add(toDelete);
            ApplyPersistContainer(pc);
        }

        protected internal abstract void CreateDataConnectionObjects();
        protected abstract void PerformApplyPersistContainer(PersistContainer container);
        




    }
}