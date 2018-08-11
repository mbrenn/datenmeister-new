using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
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
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ListViewControl, INavigationGuest
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        protected override IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope.Resolve<IViewFinder>().FindViews((Items as IHasExtent)?.Extent as IUriExtent, null);
        }

        protected override IElement RequestFormOverride(IElement selectedForm)
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            
            if (selectedForm == null)
            {
                // Nobody selected a form, so we can autocreate a new form
                if (Items == DetailItems)
                {
                    // Finds the view by the extent type
                    selectedForm = viewFinder.FindView((Items as IHasExtent)?.Extent as IUriExtent);

                }
                else
                {
                    // User has selected a sub element. 
                    selectedForm =
                        viewFinder.FindListViewFor((DetailItems as MofReflectiveSequence)?.MofObject);
                }
            }

            // Sets the generic buttons to create the new types
            if (selectedForm?.getOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
                is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    var typeName = type.get(_UML._CommonStructure._NamedElement.name);
                    AddGenericButton($"New {typeName}", () =>
                    {
                        var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent, type);
                        elements.Closed += (x, y) =>
                        {
                            UpdateContent();
                        };
                    });
                }
            }
            
            // Sets the button for the new item
            AddGenericButton("New Item", () =>
            {
                var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent);
                elements.Closed += (x, y) =>
                {
                    UpdateContent();
                };
            });

            // Adds the default button
            AddDefaultButtons();

            // Allows the deletion of an item
            AddRowItemButton(
                "Delete",
                item =>
                {
                    if (MessageBox.Show(
                            "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        _extent.elements().remove(item);
                        SetContent(_extent.elements());
                    }
                });

            return selectedForm;
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
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(workspaceId, extentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(_extent.elements());
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

        }
    }
}