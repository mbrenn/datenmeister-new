using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    public class ViewExtensionInfoCollection : ViewExtensionInfo
    {
        /// <summary>
        /// Gets or sets the reflective collection
        /// </summary>
        public IReflectiveCollection? Collection { get; set; }
        
        public ViewExtensionInfoCollection(INavigationHost navigationHost, INavigationGuest? navigationGuest) : base(navigationHost, navigationGuest)
        {
        }
    }
}