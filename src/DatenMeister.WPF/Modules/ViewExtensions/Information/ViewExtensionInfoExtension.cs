using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    /// <summary>
    /// Defines methods for view extensions
    /// </summary>
    public static class ViewExtensionInfoExtension
    {
        /// <summary>
        /// Gets the application window if the navigation host is referring to an application window
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        public static IApplicationWindow? GetMainApplicationWindow(this ViewExtensionInfo info) =>
            info is ViewExtensionInfoApplication ? info.NavigationHost as IApplicationWindow : null;


        /// <summary>
        /// Gets the information whether the event is thrown out of the ItemExplorerControl
        /// </summary>
        /// <param name="info">The target information being used</param>
        /// <returns>null, if not a valid ItemExplorerControl</returns>
        public static ItemExplorerControl? GetItemExplorerControl(this ViewExtensionInfo info) =>
            info is ViewExtensionInfoExplore ? info.NavigationGuest as ItemExplorerControl : null;

        /// <summary>
        /// Gets the information whether the target information currently indicates an ExplorerControl in
        /// which the items of an extents are shown. This event will not be filtered if thrown during the
        /// creation of the tabs.
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        /// <returns>true, if that is the case</returns>
        public static ItemsInExtentList? GetItemsInExtentExplorerControl(this ViewExtensionInfo info)
        {
            return info is ViewExtensionInfoExploreItems
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
            return info is ViewExtensionInfoExploreWorkspace
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
            return info is ViewExtensionInfoExploreExtents
                ? null
                : info.NavigationGuest as ExtentList;
        }
        
        /// <summary>
        /// Gets the list view control if it is the navigation guest
        /// </summary>
        /// <param name="info">Info to be queried</param>
        /// <returns>The found item or null</returns>
        public static ItemListViewControl? GetListViewControl(this ViewExtensionInfo info) => 
            info.NavigationGuest as ItemListViewControl;

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
            if (!(info is ViewExtensionInfoTab))
            {
                return null;
            }
            
            var extentList = info.NavigationGuest as ItemsInExtentList;
            var isCorrect = extentList != null &&
                            extentList.Extent.GetExtentType() == extentType;

            return isCorrect ? extentList : null;
        }
        
        /// <summary>
        /// Gets the detailform window, if the given view extension reflects a detail form window
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DetailFormWindow? GetDetailFormWindow(this ViewExtensionInfo info)
        {
            return info.NavigationHost as DetailFormWindow;
        }

        /// <summary>
        /// Gets the detailform control if the navigation host is the DetailFormWindow.
        /// This means that the value is only not null if a detail view is hosted within the DetailFinwo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DetailFormControl? GetDetailFormControlOfDetailWindow(this ViewExtensionInfo info) => 
            info.NavigationHost is DetailFormWindow ? info.NavigationGuest as DetailFormControl : null;
    }
}