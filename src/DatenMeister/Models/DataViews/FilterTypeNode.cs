using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.DataViews
{
    public class FilterTypeNode : ViewNode
    {
        public ViewNode? input { get; set; }
        public IElement? type { get; set; }
        public bool includeInherits { get; set; }
    }
}