﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules;
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

        public string WorkspaceId { get; set; }

        public string ExtentUrl { get; set; }

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
            Extent = extent;
            if (Extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            EventHandle = GiveMe.Scope.Resolve<ChangeEventManager>().RegisterFor(
                Extent,
                (x, y) => _delayedDispatcher.RequestRefresh());

            SetRootItem(Extent);
        }

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
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            IElement form;

            if (Equals(RootItem, SelectedItem) || SelectedItem == null)
            {
                // Finds the view by the extent type
                form = viewLogic.GetExtentForm(RootItem as IUriExtent, ViewDefinitionMode.Default);
            }
            else
            {
                // User has selected a sub element and its children shall be shown
                form = viewLogic.GetItemTreeFormForObject(
                    SelectedItem,
                    ViewDefinitionMode.Default);
            }

            const string className = "Items";
            var viewDefinition = new ViewDefinition(className, form);
            var viewExtensions = new List<ViewExtension>();

            // Sets the generic buttons to create the new types
            if (form?.GetOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
                is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    // Check if type is a directly type or the DefaultTypeForNewElement
                    if (type.metaclass.@equals(
                        _workspaceLogic.GetTypesWorkspace().Create<_FormAndFields>(
                            x => x.__DefaultTypeForNewElement)))
                    {
                        var newType = type.getOrDefault<IElement>(_FormAndFields._DefaultTypeForNewElement.metaClass);
                        var parentProperty = type.getOrDefault<string>(_FormAndFields._DefaultTypeForNewElement.parentProperty);

                        Create(newType, parentProperty);
                    }
                    else
                    {
                        Create(type, null);
                    }

                    void Create(IElement newType, string parentProperty)
                    {
                        var typeName = newType.get(_UML._CommonStructure._NamedElement.name);

                        viewExtensions.Add(new GenericButtonDefinition(
                            $"New {typeName}", () => CreateNewElementByUser(newType, parentProperty)));
                    }
                }
            }

            // Sets the button for the new item
            viewExtensions.Add(
                new GenericButtonDefinition(
                    "New Item",
                    () => CreateNewElementByUser(null, null)));

            // Allows the deletion of an item
            viewExtensions.Add(
                new RowItemButtonDefinition(
                    "Delete",
                    (guest, item) =>
                    {
                        if (MessageBox.Show(
                                "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                            MessageBoxResult.Yes)
                        {
                            Extent.elements().remove(item);
                        }
                    }));

            PrepareNavigation(viewDefinition);

            EvaluateForm(
                SelectedItem,
                new ViewDefinition(form)
                {
                    ViewExtensions =
                        viewExtensions.Union(viewDefinition.ViewExtensions).ToList()
                });
        }

        private void CreateNewElementByUser(IElement type, string parentProperty)
        {
            if (IsExtentSelectedInTreeview)
            {
                NavigatorForItems.NavigateToNewItemForExtent(
                    NavigationHost,
                    Extent,
                    type);
            }
            else
            {
                NavigatorForItems.NavigateToNewItemForItem(
                    NavigationHost,
                    SelectedPackage,
                    type,
                    parentProperty);
            }
        }

        /// <summary>
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
        /// </summary>
        public void PrepareNavigation(ViewDefinition viewDefinition)
        {
            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "To Extents",
                    () => NavigatorForExtents.NavigateToExtentList(NavigationHost, WorkspaceId),
                    Icons.ExtentsShow,
                    NavigationCategories.DatenMeister + ".Navigation"));

            viewDefinition.ViewExtensions.Add(
                new ExtentMenuButtonDefinition(
                    "Extent Info",
                    (x) => NavigatorForExtents.OpenDetailOfExtent(NavigationHost, ExtentUrl),
                    null,
                    NavigationCategories.Extents + ".Info"));

            viewDefinition.ViewExtensions.Add(
                new ExtentMenuButtonDefinition(
                    "Show as tree",
                    x => ShowAsTree(),
                    null,
                    NavigationCategories.Extents + ".Info"));

            viewDefinition.ViewExtensions.Add(
                new ExtentMenuButtonDefinition(
                    "Export as Xmi",
                    x => ExportAsXmi(),
                    null,
                    NavigationCategories.Extents + ".Export"));

            viewDefinition.ViewExtensions.Add(
                new ExtentMenuButtonDefinition(
                    "Open Extent-Folder",
                    x => OpenExtentFolder(),
                    null,
                    NavigationCategories.Extents + ".Info"));

            // Adds the infoline
            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() =>
                    new TextBlock
                    {
                        Inlines =
                        {
                            new Bold {Inlines = {new Run("Extent: ")}},
                            new Run(ExtentUrl)
                                {ContextMenu = ItemListViewControl.GetCopyToClipboardContextMenu(ExtentUrl)}
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

            void OpenExtentFolder()
            {
                var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                if (extentManager.GetLoadConfigurationFor(Extent as IUriExtent) is ExtentFileLoaderConfig
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
                    var window = new TreeViewWindow {Owner = NavigationHost.GetWindow()};
                    window.SetDefaultProperties();
                    window.SetRootItem(Extent);
                    window.ItemSelected += (x, y) =>
                        NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                    window.Show();
                }
            }
        }
    }
}