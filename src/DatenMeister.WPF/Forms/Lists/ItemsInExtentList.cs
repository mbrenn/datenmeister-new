using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using Microsoft.Win32;

namespace DatenMeister.WPF.Forms.Lists
{
    public class ItemsInExtentList : ItemExplorerControl
    {
        public ItemsInExtentList()
        {
            Loaded += ItemsInExtentList_Loaded;
            _delayedDispatcher = new DelayedRefreshDispatcher(Dispatcher, UpdateView);
            _workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
        }

        /// <summary>
        /// Gets or sets the workspace id
        /// </summary>
        public string WorkspaceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets extent Url for the item being shown
        /// </summary>
        public string ExtentUrl { get; set; } = string.Empty;

        /// <summary>
        /// Stores the delayed dispatcher
        /// </summary>
        private readonly DelayedRefreshDispatcher _delayedDispatcher;

        /// <summary>
        ///     Gets or sets a flag whether, all items shall be shown in one tab.
        /// </summary>
        public bool ShowAllItemsInOneTab { get; set; }

        private void ItemsInExtentList_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationTreeView.ShowAllChildren = false;

            _workspaceLogic.FindExtentAndWorkspace(WorkspaceId, ExtentUrl, out _, out var extent);
            if (extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            Extent = extent;
            
            EventHandle = GiveMe.Scope.Resolve<ChangeEventManager>().RegisterFor(
                Extent,
                (x, y) => _delayedDispatcher.RequestRefresh());

            SetRootItem(Extent);
        }

        /// <summary>
        /// Stores the workspace logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        ///     Sets the items of the given extent
        /// </summary>
        protected override void OnRecreateViews()
        {
            CreateFormForItems();
        }

        /// <summary>
        /// Updates all views without regenerating the tabulators, which are already set
        /// </summary>
        public override void UpdateView()
        {
            base.UpdateView();
            if (ShowAllItemsInOneTab)
            {
                return;
            }

            /*// Goes through the metaclasses and gets the one, that are not already in a tab
            var metaClasses = SelectedItems.Select(x => (x as IElement)?.getMetaClass()).Distinct().ToList();
            foreach (var metaClass in metaClasses.Where(x=> !_metaClasses.Contains(x)).ToArray())
            {
                CreateTabForMetaclass(metaClass);
            }*/
        }

        /// <summary>
        ///     Creates the tab for the given items and the metaclass that shell be shown
        /// </summary>
        private void CreateFormForItems()
        {
            var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
            var isRootItem = Equals(RootItem, SelectedItem) || SelectedItem == null;
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Require<_FormAndFields>();
                
            IElement? form = null;

            var overridingMode = OverridingViewDefinition?.Mode ?? FormDefinitionMode.Default;
            // Check if the used form shall be overridden
            if (OverridingViewDefinition != null && overridingMode == FormDefinitionMode.Specific)
            {
                // Check the type
                form = OverridingViewDefinition.Element as IElement;
                if (form != null && isRootItem)
                {
                    var formMetaClass = form.getMetaClass();
                    
                    if (!ClassifierMethods.IsSpecializedClassifierOf(
                        formMetaClass, 
                        formAndFields.__ExtentForm))
                    {
                        var formName = formMetaClass?.ToString() ?? "Unclassified";
                        
                        MessageBox.Show($"Overriding form is not of type ExtentForm, overriding form is of type {formName}");
                        form = null;
                    }
                }
            }
            
            // If the form shall not be overridden, find it by the standard logic
            if (form == null)
            {
                if (isRootItem)
                {
                    // Extent is currently selected
                    // Finds the view by the extent type
                    form = viewLogic.GetExtentForm((IUriExtent) RootItem, overridingMode);
                }
                else
                {
                    if (SelectedItem == null)
                        throw new InvalidOperationException("Not a root item, but also no SelectedItem");
                    
                    // User has selected a sub element and its children shall be shown
                    form = viewLogic.GetItemTreeFormForObject(
                        SelectedItem,
                        overridingMode);
                }
            }

            const string className = "Items";
            var viewDefinition = new FormDefinition(className, form);

            EvaluateForm(
                SelectedItem ?? throw new InvalidOperationException("No SelectedItem"),
                new FormDefinition(form ?? throw new InvalidOperationException("form == null"))
                {
                    ViewExtensions = viewDefinition.ViewExtensions
                });
        }

        public override IEnumerable<ViewExtension> GetViewExtensions()
        {
            if (NavigationHost == null) throw new InvalidOperationException("NavigationHost == null");
            var navigationHost = NavigationHost;

            yield return new ApplicationMenuButtonDefinition(
                "To Extents",
                () => NavigatorForExtents.NavigateToExtentList(navigationHost, WorkspaceId),
                Icons.ExtentsShow,
                NavigationCategories.DatenMeister + ".Navigation");

            yield return new ExtentMenuButtonDefinition(
                "Extent Info", async (x) => await NavigatorForExtents.OpenDetailOfExtent(navigationHost, ExtentUrl),
                null,
                NavigationCategories.Extents + ".Info");

            yield return new ExtentMenuButtonDefinition(
                "Extent Properties", async (x) => await NavigatorForExtents.OpenPropertiesOfExtent(navigationHost, x),
                null,
                NavigationCategories.Extents + ".Info");

            yield return new ExtentMenuButtonDefinition(
                "Show as tree",
                x => ShowAsTree(),
                null,
                NavigationCategories.Extents + ".Info");

            yield return new ExtentMenuButtonDefinition(
                "Save Extent",
                SaveExtent,
                null,
                NavigationCategories.Extents + ".Info");

            yield return new ExtentMenuButtonDefinition(
                "Export as Xmi",
                x => ExportAsXmi(),
                null,
                NavigationCategories.Extents + ".Export");

            yield return new ExtentMenuButtonDefinition(
                "Show as Xmi",
                x => ShowAsXmi(),
                null,
                NavigationCategories.Extents + ".Export");

            yield return new ExtentMenuButtonDefinition(
                "Open Extent-Folder",
                x => OpenExtentFolder(),
                null,
                NavigationCategories.Extents + ".Info");

            // Adds the infoline
            yield return new InfoLineDefinition(() =>
                new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("Extent: ")}},
                        new Run(ExtentUrl)
                            {ContextMenu = ItemListViewControl.GetCopyToClipboardContextMenu(ExtentUrl)}
                    }
                });

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
                        ExtentExport.ExportToFile(Extent, filename);
                        MessageBox.Show($"Extent exported with {Extent.elements().Count()} root elements.");
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Process.Start(Path.GetDirectoryName(filename));
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }

            void ShowAsXmi()
            {
                var dlg = new ItemXmlViewWindow();
                dlg.UpdateContent(Extent.elements());
                dlg.Show();
            }

            void OpenExtentFolder()
            {
                var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                var uriExtent = Extent as IUriExtent ?? throw new InvalidOperationException("Extent as IUriExtent");
                if (extentManager.GetLoadConfigurationFor(uriExtent) is ExtentFileLoaderConfig
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
                if (Extent != null)
                {
                    var window = new TreeViewWindow {Owner = navigationHost.GetWindow()};
                    window.SetDefaultProperties();
                    window.SetRootItem(Extent);
                    window.ItemSelected += async (x, y) =>
                        await NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                    window.Show();
                }
            }

            void SaveExtent(IExtent extent)
            {
                var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                extentManager.StoreExtent(extent);
                MessageBox.Show("Extent saved");
            }

            foreach (var extension in base.GetViewExtensions()) yield return extension;
        }

        public override ViewExtensionInfo GetViewExtensionInfo()
        {
            return new ViewExtensionInfoExploreItems(NavigationHost, this)
            {
                WorkspaceId = WorkspaceId,
                ExtentUrl = ExtentUrl,
                RootElement = RootItem,
                SelectedElement = SelectedItem
            };
        }
    }
}