#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// Contains the information about the window or dialog in which the viewextension will be shown
    /// </summary>
    public class ViewExtensionTargetInformation
    {
        /// <summary>
        /// Gets or sets the navigation host querying the view extensions
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets or sets the navigation guest which is currently
        /// </summary>
        public INavigationGuest? NavigationGuest { get; set; }

        public ViewExtensionTargetInformation(INavigationHost navigationHost, INavigationGuest? navigationGuest)
        {
            NavigationHost = navigationHost;
            NavigationGuest = navigationGuest;
        }
        
    }

    public class ViewExtensionTargetInformationForTab : ViewExtensionTargetInformation
    {
        /// <summary>
        /// Gets or sets the definition for the tab
        /// </summary>
        public IElement? TabFormDefinition { get; set; }

        public ViewExtensionTargetInformationForTab(INavigationHost navigationHost, INavigationGuest navigationGuest) 
            : base(navigationHost, navigationGuest)
        {
        }
    }

    public static class ViewExtensionTargetInformationExtension
    {
        /// <summary>
        /// Gets the information whether the event was created by the event
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        public static bool IsMainApplicationWindow(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationHost is IApplicationWindow;

        /// <summary>
        /// Gets the information whether the target information currently indicates an ExplorerControl in
        /// which the items of an extents are shown. 
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsItemsInExtentExplorerControl(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationGuest is ItemsInExtentList &&
            !(targetInformation is ViewExtensionTargetInformationForTab);
        
        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for workspace list
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsWorkspaceListExplorerControl(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationGuest is WorkspaceList&&
            !(targetInformation is ViewExtensionTargetInformationForTab);
        
        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for extent list
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsExtentListExplorerControl(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationGuest is ExtentList &&
            !(targetInformation is ViewExtensionTargetInformationForTab);
        
        public static bool IsListViewForItemsTab(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationGuest is ItemsInExtentList &&
            targetInformation is ViewExtensionTargetInformationForTab;

        /// <summary>
        /// Checks, if a tab is created for the items within in an extent having a certain extent type 
        /// </summary>
        /// <param name="targetInformation"></param>
        /// <param name="extentType"></param>
        /// <returns></returns>
        public static ItemsInExtentList? GetListViewForItemsTabForExtentType(
            this ViewExtensionTargetInformation targetInformation,
            string extentType)

        {
            var extentList = targetInformation.NavigationGuest as ItemsInExtentList;
            var isCorrect =  extentList != null && 
                targetInformation is ViewExtensionTargetInformationForTab &&
                extentList.Extent?.GetExtentType() == extentType;

            return isCorrect ? extentList : null;
        }
    }

    /// <summary>
    /// This class is used when an item list is required to show all properties of the given item.
    /// Here, the plugins are asked whether a view extension shall be shown for these items.
    /// </summary>
    public class ViewExtensionForItemPropertiesInformation : ViewExtensionTargetInformation
    {
        /// <summary>
        /// Gets or sets the value which is intended to be shown
        /// </summary>
        public IObject? Value { get; set; }
        
        /// <summary>
        /// Gets or sets the property which is queried to be shown
        /// </summary>
        public string? Property { get; set; }

        public ViewExtensionForItemPropertiesInformation(INavigationHost navigationHost, INavigationGuest navigationGuest) : base(navigationHost, navigationGuest)
        {
        }
    }
}