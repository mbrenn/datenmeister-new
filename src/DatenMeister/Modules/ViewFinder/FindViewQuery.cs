using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;

namespace DatenMeister.Modules.ViewFinder
{
    public class FindViewQuery
    {
        public ViewType viewType { get; set; }
        public IElement metaClass { get; set; }
        public string extentType { get; set; }

        public IElement parentMetaClass { get; set; }

        public string parentProperty { get; set; }
    }
}