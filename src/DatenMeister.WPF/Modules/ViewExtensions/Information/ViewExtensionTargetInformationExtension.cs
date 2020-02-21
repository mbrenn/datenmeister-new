using DatenMeister.Runtime;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    /// <summary>
    /// Defines methods for view extensions
    /// </summary>
    public static class ViewExtensionTargetInformationExtension
    {
        /// <summary>
        /// Gets the application window if the navigation host is referring to an application window
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        public static IApplicationWindow?
            GetMainApplicationWindow(this ViewExtensionInfo info) =>
            info.NavigationHost as IApplicationWindow;

        
        /// <summary>
        /// Gets the information whether the event is thrown out of the ItemExplorerControl
        /// </summary>
        /// <param name="info">The target information being used</param>
        /// <returns>null, if not a valid ItemExplorerControl</returns>
        public static ItemExplorerControl? GetExplorerControl(this ViewExtensionInfo info) =>
            info.NavigationGuest as ItemExplorerControl;

        /// <summary>
        /// Gets the information whether the target information currently indicates an ExplorerControl in
        /// which the items of an extents are shown. This event will not be filtered if thrown during the
        /// creation of the tabs
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static ItemsInExtentList? GetItemsInExtentExplorerControl(this ViewExtensionInfo info)
        {
            return info is ViewExtensionInfoTab
                ? null
                : info.NavigationGuest as ItemsInExtentList;
        }

        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for workspace list
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static WorkspaceList? GetWorkspaceListExplorerControl(this ViewExtensionInfo info)
        {
            return info is ViewExtensionInfoTab
                ? null
                : info.NavigationGuest as WorkspaceList;
        }
        
        /// <summary>
        /// Gets the information whether the event is sent out of the explorer control for extent list
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static ExtentList? GetExtentListExplorerControl(this ViewExtensionInfo info)
        {
            return info is ViewExtensionInfoTab
                ? null
                : info.NavigationGuest as ExtentList;
        }

        public static ItemsInExtentList? GetListViewForItemsTab(this ViewExtensionInfo info)
        {
            return info is ViewExtensionInfoTab
                ? info.NavigationGuest as ItemsInExtentList
                : null;
        }

        /// <summary>
        /// Checks, if a tab is created for the items within in an extent having a certain extent type.
        /// </summary>
        /// <param name="info">The target information to be used</param>
        /// <param name="extentType">The extent type which is requested</param>
        /// <returns>null, if the item is not fitting or the ItemsInExtentList if the
        /// extent type is fitting</returns>
        public static ItemsInExtentList? GetListViewForItemsTabForExtentType(
            this ViewExtensionInfo info,
            string extentType)
        {
            var extentList = info.NavigationGuest as ItemsInExtentList;
            var isCorrect = extentList != null &&
                            info is ViewExtensionInfoTab &&
                            extentList.Extent.GetExtentType() == extentType;

            return isCorrect ? extentList : null;
        }
    }
}