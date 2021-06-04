using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormFinder;
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
            _delayedDispatcher = new DelayedRefreshDispatcher(Dispatcher, UpdateForm);
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
            
            EventHandle = GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>().RegisterFor(
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
        protected override void OnRecreateForms()
        {
            CreateFormForItems();
        }

        /// <summary>
        /// Updates all views without regenerating the tabulators, which are already set
        /// </summary>
        public override void UpdateForm()
        {
            base.UpdateForm();
            if (ShowAllItemsInOneTab)
            {
            }
        }

        /// <summary>
        /// Creates the tab for the given items and the metaclass that shell be shown
        /// </summary>
        private void CreateFormForItems()
        {
            var formPlugin = GiveMe.Scope.Resolve<FormsPlugin>();
            var isRootItem = Equals(RootItem, SelectedItem) || SelectedItem == null;
                
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
                        _DatenMeister.TheOne.Forms.__ExtentForm))
                    {
                        var formType = formMetaClass?.ToString() ?? "Unclassified";
                        
                        MessageBox.Show($"Overriding form is not of type ExtentForm, overriding form is of type {formType}");
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
                    form = formPlugin.GetExtentForm((IUriExtent) RootItem, overridingMode, CurrentViewModeId);
                }
                else
                {
                    if (SelectedItem == null)
                        throw new InvalidOperationException("Not a root item, but also no SelectedItem");

                    var viewMode = CurrentViewModeId;
                    if (DefaultClassifierHints.IsPackageLike(SelectedItem))
                    {
                        viewMode = SelectedItem.getOrDefault<string>(
                                       _DatenMeister._CommonTypes._Default._Package.defaultViewMode)
                                   ?? viewMode;
                    }
                    
                    // User has selected a sub element and its children shall be shown
                    form = formPlugin.GetItemTreeFormForObject(
                        SelectedItem,
                        overridingMode, 
                        viewMode);
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
                        
                        DotNetHelper.CreateProcess(Path.GetDirectoryName(filename)!);
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
                var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
                var uriExtent = Extent as IUriExtent ?? throw new InvalidOperationException("Extent as IUriExtent");
                var loadConfiguration = extentManager.GetLoadConfigurationFor(uriExtent);
                if (loadConfiguration != null && loadConfiguration.isSet(_DatenMeister._ExtentLoaderConfigs._ExtentFileLoaderConfig.filePath) == true)
                {
                    var filePath = loadConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentFileLoaderConfig.filePath);
                    
                    //Clean up file path so it can be navigated OK
                    filePath = Path.GetFullPath(filePath);
                    DotNetHelper.CreateProcess("explorer.exe", $"/select,\"{filePath}\"");
                }
                else
                {
                    MessageBox.Show("Given extent is not file-driven (probably only in memory).");
                }
            }

            void ShowAsTree()
            {
                if (Extent != null && navigationHost != null)
                {
                    var window = new TreeViewWindow {Owner = navigationHost.GetWindow()};
                    window.SetDefaultProperties();
                    window.SetRootItem(Extent);
                    window.ItemSelected += async (x, y) =>
                    {
                        if (y.Item is IExtent asExtent)
                        {
                            await NavigatorForExtents.OpenPropertiesOfExtent(NavigationHost, asExtent);
                        }
                        else
                        {
                            await NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                        }
                    };
                    window.Show();
                }
            }

            foreach (var extension in base.GetViewExtensions()) yield return extension;
        }

        private void SaveExtent(IExtent extent)
        {
            var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
            extentManager.StoreExtent(extent);
            
            UpdateTreeContent();
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!(RootItem is IExtent extent))
            {
                MessageBox.Show("The root item is not an extent");
                return;
            }
            
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.S)
                {
                    SaveExtent(extent);   
                }
            }
            
            base.OnKeyDown(e);
        }
    }
}