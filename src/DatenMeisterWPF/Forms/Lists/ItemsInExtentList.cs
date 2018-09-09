using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ItemExplorerControl, INavigationGuest
    {
        private IExtent _extent;

        public string WorkspaceId { get; set; }
        public string ExtentUrl { get; set; }

        public ItemsInExtentList()
        {
            Loaded += ItemsInExtentList_Loaded;
        }

        private void ItemsInExtentList_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent(WorkspaceId, ExtentUrl);
        }

        /// <summary>
        /// Sets the items of the given extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be used</param>
        public void SetContent(string workspaceId, string extentUrl)
        {
            WorkspaceId = workspaceId;
            ExtentUrl = extentUrl;
            var viewFinder = App.Scope.Resolve<IViewFinder>();

            IElement view = null;
                // Nobody selected a form, so we can autocreate a new form
            if (Items == SelectedItems)
            {
                // Finds the view by the extent type
                view = viewFinder.FindView((Items as IHasExtent)?.Extent as IUriExtent);
            }
            else
            {
                // User has selected a sub element. 
                view =
                    viewFinder.FindListViewFor((SelectedItems as MofReflectiveSequence)?.MofObject);
            }
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(workspaceId, extentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            var element = AddTab(
                _extent.elements(),
                new ViewDefinition(
                    "Extent",
                    view));
            
            // Sets the generic buttons to create the new types
            if (view?.getOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
                is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    var typeName = type.get(_UML._CommonStructure._NamedElement.name);
                    element.Control.AddGenericButton($"New {typeName}", () =>
                    {
                        var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent, type);
                        elements.Closed += (x, y) =>
                        {
                            RecreateViews();
                        };
                    });
                }
            }

            // Sets the button for the new item
            element.Control.AddGenericButton("New Item", () =>
            {
                var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent);
                elements.Closed += (x, y) =>
                {
                    RecreateViews();
                };
            });

            // Adds the default button
            element.Control.AddDefaultButtons();

            // Allows the deletion of an item
            element.Control.AddRowItemButton(
                "Delete",
                (guest, item) =>
                {
                    if (MessageBox.Show(
                            "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        _extent.elements().remove(item);
                        element.Control.UpdateContent();
                    }
                });
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public new void PrepareNavigation()
        {
            base.PrepareNavigation();

            NavigationHost.AddNavigationButton(
                "To Extents",
                () => NavigatorForExtents.NavigateToExtentList(NavigationHost, WorkspaceId),
                Icons.ExtentsShow,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Extent Info",
                () => NavigatorForExtents.OpenExtent(NavigationHost, WorkspaceId, ExtentUrl),
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Show as tree",
                () =>
                {
                    if (_extent != null)
                    {
                        var window = new TreeViewWindow();
                        window.Owner = NavigationHost.GetWindow();
                        window.SetDefaultProperties();
                        window.SetCollection(_extent.elements());
                        window.ItemSelected += (x, y) =>
                        {
                            NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                        };
                        window.Show();
                    }
                },
                null,
                NavigationCategories.File + ".Views");

            NavigationHost.AddNavigationButton(
                "Open Extent-Folder",
                () =>
                {
                    var extentManager = App.Scope.Resolve<IExtentManager>();
                    if (
                        extentManager.GetLoadConfigurationFor(_extent as IUriExtent)
                            is ExtentFileLoaderConfig loadConfiguration
                        && loadConfiguration.Path != null)
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Process.Start(
                            Path.GetDirectoryName(loadConfiguration.Path));
                    }
                    else
                    {
                        MessageBox.Show("Given extent is not file-driven (probably only in memory).");
                    }
                },
                null,
                NavigationCategories.File + ".Workspaces");
            
            AddInfoLine(
                new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("Extent: ")}},
                        new Run(ExtentUrl)
                    }
                });
        }
    }
}