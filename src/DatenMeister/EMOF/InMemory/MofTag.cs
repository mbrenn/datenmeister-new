using DatenMeister.EMOF.Interface.Extension;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    public class MofTag : ITag
    {
        public string name
        {
            get;
            set;
        }

        public string value
        {
            get;
            set;
        }
        
        public IReflectiveCollection elements
        {
            get;
            set;
        }

        public IElement owner
        {
            get;
            set;
        }

    }
}
