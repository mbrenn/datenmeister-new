#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.ManagementProvider;
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

    /// <summary>
    /// This class is instantiated for each tab of the ItemExplorerTab
    /// </summary>
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

    /// <summary>
    /// Defines methods for view extensions
    /// </summary>
    public static class ViewExtensionTargetInformationExtension
    {
        /// <summary>
        /// Gets the application window if the navigation host is referring to an application window
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        public static IApplicationWindow?
            GetMainApplicationWindow(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationHost as IApplicationWindow;

        
        /// <summary>
        /// Gets the information whether the event is thrown out of the ItemExplorerControl
        /// </summary>
        /// <param name="targetInformation">The target information being used</param>
        /// <returns>null, if not a valid ItemExplorerControl</returns>
        public static ItemExplorerControl? GetExplorerControl(this ViewExtensionTargetInformation targetInformation) =>
            targetInformation.NavigationGuest as ItemExplorerControl;

        /// <summary>
        /// Gets the information whether the target information currently indicates an ExplorerControl in
        /// which the items of an extents are shown. This event will not be filtered if thrown during the
        /// creation of the tabs
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static ItemsInExtentList? GetItemsInExtentExplorerControl(this ViewExtensionTargetInformation targetInformation)
        {
            return targetInformation is ViewExtensionTargetInformationForTab
                ? null
                : targetInformation.NavigationGuest as ItemsInExtentList;
        }

        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for workspace list
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static WorkspaceList? GetWorkspaceListExplorerControl(this ViewExtensionTargetInformation targetInformation)
        {
            return targetInformation is ViewExtensionTargetInformationForTab
                ? null
                : targetInformation.NavigationGuest as WorkspaceList;
        }
        
        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for extent list
        /// </summary>
        /// <param name="targetInformation">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static ExtentList? GetExtentListExplorerControl(this ViewExtensionTargetInformation targetInformation)
        {
            return targetInformation is ViewExtensionTargetInformationForTab
                ? null
                : targetInformation.NavigationGuest as ExtentList;
        }

        public static ItemsInExtentList? GetListViewForItemsTab(this ViewExtensionTargetInformation targetInformation)
        {
            return targetInformation is ViewExtensionTargetInformationForTab
                ? targetInformation.NavigationGuest as ItemsInExtentList
                : null;
        }

        /// <summary>
        /// Checks, if a tab is created for the items within in an extent having a certain extent type.
        /// </summary>
        /// <param name="targetInformation">The target information to be used</param>
        /// <param name="extentType">The extent type which is requested</param>
        /// <returns>null, if the item is not fitting or the ItemsInExtentList if the
        /// extent type is fitting</returns>
        public static ItemsInExtentList? GetListViewForItemsTabForExtentType(
            this ViewExtensionTargetInformation targetInformation,
            string extentType)
        {
            var extentList = targetInformation.NavigationGuest as ItemsInExtentList;
            var isCorrect = extentList != null &&
                            targetInformation is ViewExtensionTargetInformationForTab &&
                            extentList.Extent.GetExtentType() == extentType;

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