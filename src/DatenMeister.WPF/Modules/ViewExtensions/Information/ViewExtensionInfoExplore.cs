using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

public class ViewExtensionInfoExplore : ViewExtensionInfo
{
    public ViewExtensionInfoExplore(INavigationHost navigationHost, INavigationGuest? navigationGuest) : base(navigationHost, navigationGuest)
    {
    }
        
    /// <summary>
    /// Gets or sets the root element 
    /// </summary>
    public IObject? RootElement { get; set; }
        
    /// <summary>
    /// Gets or sets the selected element
    /// </summary>
    public IObject? SelectedElement { get; set; }
}