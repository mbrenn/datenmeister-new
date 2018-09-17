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
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ItemExplorerControl
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
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(WorkspaceId, ExtentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetItems(_extent.elements());
        }

        /// <summary>
        /// Sets the items of the given extent
        /// </summary>
        protected override void OnRecreateViews()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();

            // Group by SelectedItems
            var metaClasses = SelectedItems.Select(x => (x as IElement)?.getMetaClass()).Distinct();
            foreach (var metaClass in metaClasses)
            {
                IReflectiveCollection tabItems;
                if (metaClass == null)
                {
                    tabItems = SelectedItems.WhenMetaClassIsNotSet();
                }
                else
                {
                    tabItems = SelectedItems.WhenMetaClassIsOneOf(metaClass);
                }

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
                        viewFinder.FindListViewFor((tabItems as MofReflectiveSequence)?.MofObject)
                        ?? viewFinder.CreateView(tabItems);
                }

                var className = metaClass == null ? "Items" : UmlNameResolution.GetName(metaClass);
                view?.set("name", className);
                var viewDefinition = new ViewDefinition(className, view);

                // Sets the generic buttons to create the new types
                if (view?.getOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
                    is IReflectiveCollection defaultTypesForNewItems)
                {
                    foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                    {
                        var typeName = type.get(_UML._CommonStructure._NamedElement.name);

                        viewDefinition.ViewExtensions.Add(new GenericButtonDefintion(
                            $"New {typeName}", () =>
                            {
                                var elements =
                                    NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent, type);
                                elements.Closed += (x, y) => { RecreateViews(); };
                            }));
                    }
                }

                // Sets the button for the new item
                viewDefinition.ViewExtensions.Add(new GenericButtonDefintion(
                    "New Item", () =>
                    {
                        var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent);
                        elements.Closed += (x, y) => { RecreateViews(); };
                    }));

                // Allows the deletion of an item
                viewDefinition.ViewExtensions.Add(new RowItemButtonDefinition(
                    "Delete",
                    (guest, item) =>
                    {
                        if (MessageBox.Show(
                                "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                            MessageBoxResult.Yes)
                        {
                            _extent.elements().remove(item);
                            //element.Control.UpdateContent();
                        }
                    }));

                PrepareNavigation(viewDefinition);

                var element = AddTab(
                    tabItems,
                    viewDefinition);
            }
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public void PrepareNavigation(ViewDefinition viewDefinition)
        {
            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "To Extents",
                    () => NavigatorForExtents.NavigateToExtentList(NavigationHost, WorkspaceId),
                    Icons.ExtentsShow,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Extent Info",
                    () => NavigatorForExtents.OpenExtent(NavigationHost, WorkspaceId, ExtentUrl),
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
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
                    NavigationCategories.File + ".Views"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
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
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() =>
                    new TextBlock
                    {
                        Inlines =
                        {
                            new Bold {Inlines = {new Run("Extent: ")}},
                            new Run(ExtentUrl)
                        }
                    }));
        }
    }
}