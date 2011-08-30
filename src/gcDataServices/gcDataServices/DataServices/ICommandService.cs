using System.Collections.Generic;

namespace GeniusCode.Components.DataServices
{
    public static class CommandServiceExtensions
    {
        public static  void SaveObject(this ICommandService container, object toSave)
        {
            var pc = new PersistContainer();
            pc.ToSave.Add(toSave);
            container.ApplyPersistContainer(pc);
        }

        public static void DeleteObject(this ICommandService container, object toDelete)
        {
            var pc = new PersistContainer();
            pc.ToDelete.Add(toDelete);
            container.ApplyPersistContainer(pc);
        }
    }

    public interface ICommandService
    {
        void ApplyPersistContainer(PersistContainer container);
        
    }

    public class PersistContainer
    {
        public PersistContainer()
        {
            ToSave = new List<object>();
            ToDelete = new List<object>();
        }

        public List<object> ToSave { get; private set; }
        public List<object> ToDelete { get; private set; }
    }
}