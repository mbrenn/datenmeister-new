using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
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
        /// Gets the information whether the event is sent out of the explorer control for extent list
        /// </summary>
        /// <param name="info">TargetInformation to be used</param>
        /// <param name="rootExtentType">Defines the extent type which shall be used as a filter</param>
        /// <returns>true, if that is the case</returns>
        public static ItemExplorerControl? GetItemExplorerControlForExtentType(this ViewExtensionInfo info, string? rootExtentType)
        {
            var extentList = info is ViewExtensionInfoExploreExtents
                ? null
                : info.NavigationGuest as ItemExplorerControl;

            var isCorrect = extentList != null &&
                            (rootExtentType == null ||
                             extentList.Extent.GetConfiguration().ContainsExtentType(rootExtentType));

            return isCorrect ? extentList : null;
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
                            extentList.Extent.GetConfiguration().ContainsExtentType(extentType);

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
        /// Gets the detailform window, if the given view extension reflects a detail form window
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DetailFormControl? GetDetailFormControl(this ViewExtensionInfo info)
        {
            return info.NavigationGuest as DetailFormControl;
        }

        /// <summary>
        /// Checks whether the shown item in the detail window form is of a specific metaclass  
        /// </summary>
        /// <param name="info">ViewExtension information</param>
        /// <param name="metaclass">The Metaclass against which the value is queried</param>
        /// <param name="followGeneralizations">true, if the generalizations shall also be followed</param>
        /// <returns>The found detail form window and the retrieved element or null, if not found</returns>
        public static (DetailFormControl, IElement)? IsItemInDetailWindowOfType(
            this ViewExtensionInfo info,
            IElement metaclass,
            bool followGeneralizations = false)
        {
            var detailFormControl = info.GetDetailFormControl();
            if (detailFormControl == null)
            {
                return null;
            }

            if (!(detailFormControl.DetailElement is IElement element))
            {
                return null;
            }

            var itemMetaClass = element.getMetaClass();
            if (itemMetaClass == null)
            {
                return null;
            }

            if (itemMetaClass.equals(metaclass)
                || followGeneralizations
                   && ClassifierMethods.IsSpecializedClassifierOf(itemMetaClass, metaclass))
            {
                return (detailFormControl, element);
            }
            
            return null;
        }

        /// <summary>
        /// Gets the detailform control if the navigation host is the DetailFormWindow.
        /// This means that the value is only not null if a detail view is hosted within the DetailFinwo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DetailFormControl? GetDetailFormControlOfDetailWindow(this ViewExtensionInfo info) => 
            info.NavigationHost is DetailFormWindow ? info.NavigationGuest as DetailFormControl : null;

        /// <summary>
        /// Checks whether the selected item is a root item 
        /// </summary>
        /// <param name="info">View extension to be used</param>
        /// <param name="extentType">true only, if the extent item is of the given extent. If null,
        /// then it does not matter which extent type is set</param>
        /// <returns></returns>
        public static bool IsExtentSelectedInExplorerControl(this ViewExtensionInfo info, string? extentType = null)
        {
            return GetItemExplorerControlForExtentType(info, extentType) != null;
        }

        /// <summary>
        /// Gets the information whether the view extension reflects a listview control and an extent is checked
        /// </summary>
        /// <param name="info">Information to be used</param>
        /// <param name="extentType">Type of the extent</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsExtentInListViewControl(this ViewExtensionInfo info, string? extentType = null)
        {
            if (info is ViewExtensionInfoTab extensionTab
                && extensionTab.NavigationGuest is ItemExplorerControl explorerControl)
            {
                var selectedItem = explorerControl.SelectedItem;
                if ((selectedItem ?? explorerControl.RootItem) is IExtent asExtent)
                {
                    if (extentType != null && !asExtent.GetConfiguration().ContainsExtentType(extentType))
                    {
                        return false;
                    }
                    
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the information whether the view extension reflects a listview control and an extent is checked
        /// </summary>
        /// <param name="info">Information to be used</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="metaClasses">Only true, if the element is in one of the metaclasses</param>
        /// <param name="extentType">Extent type to be used</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsItemOfExtentTypeInListViewControl(this ViewExtensionInfo info, string propertyName,
            IEnumerable<IElement>? metaClasses = null, string? extentType = null)
        {
            if (info is ViewExtensionInfoTab extensionTab
                && extensionTab.NavigationGuest is ItemExplorerControl explorerControl)
            {
                var formPropertyName = extensionTab.TabFormDefinition.getOrDefault<string>(_DatenMeister._Forms._TableForm.property);
                if (!((explorerControl.SelectedItem ?? explorerControl.RootItem) is IElement selectedItem)) return false; // Nothing selected, should not occur
                
                // Checks the extent type
                var extentConfiguration = selectedItem.GetExtentOf()?.GetConfiguration();
                
                if (extentType != null)
                {
                    if (extentConfiguration == null || !extentConfiguration.ContainsExtentType(extentType))
                    {
                        return false;
                    }
                }
                
                // Checks for property name
                if (!string.IsNullOrEmpty(formPropertyName) && formPropertyName != propertyName)
                {
                    return false;
                }

                // Checks the metaclasses
                if (metaClasses != null 
                    && !metaClasses.Any(x => x.equals(selectedItem.getMetaClass())))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}