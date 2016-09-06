using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Performs an allocation between the view and a specific metaclass which supports the retrieval of default views. 
    /// </summary>
    public class DefaultViewForMetaclass
    {
        public ViewType viewType { get; set; } 

        public IElement metaclass { get; set; }
        
        public IElement view { get; set; }
    }
}