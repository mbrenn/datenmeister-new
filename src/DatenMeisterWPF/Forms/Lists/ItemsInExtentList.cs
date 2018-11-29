using System;
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
using DatenMeister.Runtime.Extents;
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
using Microsoft.Win32;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ItemExplorerControl
    {
        private IExtent _extent;

        public ItemsInExtentList()
        {
            Loaded += ItemsInExtentList_Loaded;
        }

        public string WorkspaceId { get; set; }
        public string ExtentUrl { get; set; }

        /// <summary>
        ///     Gets or sets a flag whether, all items shall be shown in one tab.
        /// </summary>
        public bool ShowAllItemsInOneTab { get; set; }

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
        ///     Sets the items of the given extent
        /// </summary>
        protected override void OnRecreateViews()
        {
            // Group by SelectedItems
            var metaClasses = SelectedItems.Select(x => (x as IElement)?.getMetaClass()).Distinct().ToList();
            if (metaClasses.Count == 0 || ShowAllItemsInOneTab)
            {
                CreateTabForItems(SelectedItems, null);
            }
            else
            {
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

                    CreateTabForItems(tabItems, metaClass);
                }
            }
        }

        /// <summary>
        ///     Creates the tab for the given items and the metaclass that shell be shown
        /// </summary>
        /// <param name="tabItems">Items for the tab</param>
        /// <param name="metaClass">Meta class of the items</param>
        private void CreateTabForItems(IReflectiveCollection tabItems, IElement metaClass)
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            IElement view;

            if (Items == SelectedItems)
            {
                // Finds the view by the extent type
                view = viewFinder.FindListView((Items as IHasExtent)?.Extent as IUriExtent, metaClass);
            }
            else
            {
                // User has selected a sub element and its children shall be shown
                view =
                    viewFinder.FindListViewFor(
                        (tabItems as MofReflectiveSequence)?.MofObject, metaClass)
                    ?? viewFinder.CreateView(tabItems);
            }

            var className = metaClass == null ? "Items" : NamedElementMethods.GetName(metaClass);
            view?.set("name", className);

            var viewDefinition = new ViewDefinition(className, view);

            // Sets the generic buttons to create the new types
            if (view?.GetOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
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

        /// <summary>
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
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
                    ShowAsTree,
                    null,
                    NavigationCategories.File + ".Views"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Export as Xmi",
                    ExportAsXmi,
                    null,
                    NavigationCategories.File + ".Export"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Open Extent-Folder",
                    OpenExtentFolder,
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

            void ExportAsXmi()
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Xmi-File (*.xmi)|*.xmi|Xml-Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    AddExtension = true,
                    RestoreDirectory = true
                };
                if (dialog.ShowDialog() == true)
                {
                    var filename = dialog.FileName;
                    try
                    {
                        ExtentExport.ExportToFile(_extent, filename);
                        MessageBox.Show($"Extent exported with {_extent.elements().Count()} root elements.");
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Process.Start(Path.GetDirectoryName(filename));
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }

            void OpenExtentFolder()
            {
                var extentManager = App.Scope.Resolve<IExtentManager>();
                if (extentManager.GetLoadConfigurationFor(_extent as IUriExtent) is ExtentFileLoaderConfig
                        loadConfiguration && loadConfiguration.filePath != null)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Process.Start(Path.GetDirectoryName(loadConfiguration.filePath));
                }
                else
                {
                    MessageBox.Show("Given extent is not file-driven (probably only in memory).");
                }
            }

            void ShowAsTree()
            {
                if (_extent != null)
                {
                    var window = new TreeViewWindow {Owner = NavigationHost.GetWindow()};
                    window.SetDefaultProperties();
                    window.SetCollection(_extent.elements());
                    window.ItemSelected += (x, y) =>
                    {
                        NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                    };
                    window.Show();
                }
            }
        }
    }
}