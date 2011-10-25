using System.Collections.Generic;
using System.Linq;

namespace GeniusCode.Components.DataServices
{
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