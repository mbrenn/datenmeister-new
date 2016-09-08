using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.InMemory
{
    public class MofTag : ITag
    {
        public string name { get; set; }

        public string value { get; set; }

        public IReflectiveCollection elements { get; set; }

        public IElement owner { get; set; }
    }
}