﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.XPath;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemsTreeView.xaml
    /// </summary>
    public partial class ItemsTreeView : UserControl, INavigationGuest
    {
        /// <summary>
        /// Defines the logger being used
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(ItemsTreeView));
        
        /// <summary>
        /// Defines the element which is the source for the definition of the treeview
        /// </summary>
        private IObject? _itemsSource;

        /// <summary>
        /// Defines the set of already visited items to prevent that one item is 'visited' multiple times
        /// </summary>
        private readonly HashSet<object> _alreadyVisited = new HashSet<object>();

        public static readonly DependencyProperty ShowRootProperty = DependencyProperty.Register(
            "ShowRoot", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ShowMetaClassesProperty = DependencyProperty.Register(
            "ShowMetaClasses", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool), OnShowMetaClassesChange));

        private static void OnShowMetaClassesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ItemsTreeView itemsTreeView))
            {
                throw new InvalidOperationException("Dependency object is not of type ItemsTreeView");
            }
            
            itemsTreeView.UpdateForm();
            itemsTreeView._cacheShowMetaClasses = (bool) e.NewValue;
        }

        public bool ShowMetaClasses
        {
            get => (bool) GetValue(ShowMetaClassesProperty);
            set => SetValue(ShowMetaClassesProperty, value);
        }

        private bool _cacheShowMetaClasses;

        /// <summary>
        /// Gets a list of the viewextension for the tree view
        /// </summary>
        public List<ViewExtension> ViewExtensions { get; } = new List<ViewExtension>();

        /// <summary>
        /// Gets or sets a value indicating whether a root element shall be introduced
        /// </summary>
        public bool ShowRoot
        {
            get => (bool) GetValue(ShowRootProperty);
            set
            {
                SetValue(ShowRootProperty, value);
                UpdateForm();
            }
        }

        public static readonly DependencyProperty ShowAllChildrenProperty = DependencyProperty.Register(
            "ShowAllChildren", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool)));

        public bool ShowAllChildren
        {
            get => (bool) GetValue(ShowAllChildrenProperty);
            set
            {
                if (ShowAllChildren != value)
                {
                    SetValue(ShowAllChildrenProperty, value);
                    UpdateForm();
                }
            }
        }

        /// <summary>
        /// Stores the metaclasses that will be used as filtering
        /// </summary>
        private IEnumerable<IElement>? _filterMetaClasses;

        /// <summary>
        /// Gets or sets the metaclasses that will be used as filtering
        /// </summary>
        public IEnumerable<IElement>? FilterMetaClasses
        {
            get => _filterMetaClasses;
            set
            {
                _filterMetaClasses = value;
                UpdateForm();
            }
        }

        /// <summary>
        /// Defines the maximum items per level. This is used to reduce the amount of items
        /// </summary>
        private const int MaxItemsPerLevel = 100;

        /// <summary>
        /// Stores the properties being used to retrieve the items
        /// </summary>
        private readonly HashSet<string> _propertiesForChildren = new HashSet<string>();

        private readonly Dictionary<IObject, TreeViewItem> _mappingItems = new Dictionary<IObject, TreeViewItem>();

        /// <summary>
        /// Stores the previously selected item when the tree is rebuilt.
        /// This allows the refocussing to the item
        /// </summary>
        private object? _previouslySelectedItem;

        /// <summary>
        /// Stores the item that shall be selected, after the items have been created
        /// </summary>
        private TreeViewItem? _newSelectedItem;

        /// <summary>
        /// Gets the root element
        /// </summary>
        public IObject? RootElement => _itemsSource;

        /// <summary>
        /// Stores the list of hints for the default classifier
        /// </summary>
        private readonly DefaultClassifierHints _defaultClassifierHints;

        private INavigationHost? _navigationHost;

        public ItemsTreeView()
        {
            InitializeComponent();
            _defaultClassifierHints = new DefaultClassifierHints(GiveMe.Scope.WorkspaceLogic);
        }

        public IObject? ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                UpdateForm();
            }
        }

        public void SetSelectedItem(IObject? value)
        {
            if (value != null && _mappingItems.TryGetValue(value, out var treeviewItem))
            {
                treeviewItem.IsSelected = true;
            }
        }

        public IObject? GetSelectedItem()
        {
            if (TreeView.SelectedItem is TreeViewItem treeViewItem)
            {
                return treeViewItem.Tag as IObject;
            }

            return null;
        }

        /// <summary>
        /// Adds a property as a child property.
        /// </summary>
        /// <param name="properties">Properties which shall be added reflecting a child property</param>
        public void AddPropertyForChild(params string[] properties)
        {
            foreach (var property in properties)
            {
                _propertiesForChildren.Add(property);
            }

            UpdateForm();
        }

        /// <summary>
        /// Sets the default properties for the view.
        /// The default property is "packagedElement" as child package for Packages
        /// </summary>
        public void SetDefaultProperties()
        {
            _propertiesForChildren.Add(_UML._Packages._Package.packagedElement);
        }

        /// <summary>
        /// Updates the complete view of the item tree
        /// </summary>
        public void UpdateForm()
        {
            if (!IsInitialized)
            {
                // Save the time...
                return;
            }

            using var watch = new StopWatchLogger(Logger, "UpdateView", LogLevel.Trace);

            if (ItemsSource == null)
            {
                TreeView.ItemsSource = null;
                return;
            }

            var model = new List<TreeViewItem>();

            _newSelectedItem = null;
            _previouslySelectedItem = (TreeView.SelectedItem as TreeViewItem)?.Tag;

            var container = TreeView as ItemsControl;
            lock (_alreadyVisited)
            {
                _alreadyVisited.Clear();
                _mappingItems.Clear();

                // Checks, if a tree views are already created
                var availableTreeViewItem = (container.ItemsSource as List<TreeViewItem>)?.FirstOrDefault();
                if (availableTreeViewItem == null)
                {
                    var found = CreateTreeViewItem(ItemsSource, true);
                    if (found != null)
                        model.Add(found);
                    
                    container.ItemsSource = model;
                }
                else
                {
                    UpdateTreeViewItem(availableTreeViewItem, ItemsSource);
                }
            }

            if (_newSelectedItem != null)
            {
                _newSelectedItem.IsSelected = true;
                _newSelectedItem.IsExpanded = true;
                _newSelectedItem.BringIntoView();
            }
        }

        /// <summary>
        /// Updates the treeview item by using the current item and compares it to the given item
        /// which should be matching to the treeview item
        /// </summary>
        /// <param name="availableTreeViewItem"></param>
        /// <param name="item"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateTreeViewItem(TreeViewItem availableTreeViewItem, IObject item)
        {
            // Updates the current item
            availableTreeViewItem.Header = GetItemHeader(item);
            availableTreeViewItem.Tag = item;

            var childrenOfItem = GetChildrenOfItem(item).ToList();
            
            if (availableTreeViewItem.ItemsSource is List<TreeViewItem> viewChildren)
            {
                foreach (var viewChild in viewChildren.ToList())
                {
                    if (!(viewChild.Tag is IObject viewChildItem))
                    {
                        // If the tag is not an IObject, remove it
                        // Should not happen, but who knows
                        viewChildren.Remove(viewChild);
                        continue;
                    }
                    
                    // Checks, if the given element is in the children of Item
                    if (childrenOfItem.FirstOrDefault(x => viewChildItem.@equals(x))
                        is IObject found)
                    {
                        // If the element was found, add it
                        UpdateTreeViewItem(viewChild, found);
                        childrenOfItem.Remove(found);
                    }
                    else
                    {
                        // If the element is not found, remove it
                        viewChildren.Remove(viewChild);
                    }
                }
                
                // Now add the items which are left
                foreach (var child in childrenOfItem)
                {
                    if (child is null)
                        continue;

                    var createdTreeViewItem = CreateTreeViewItem(child);
                    if (createdTreeViewItem != null)
                    {
                        viewChildren.Add(createdTreeViewItem);
                    }
                }

                availableTreeViewItem.ItemsSource = viewChildren;
                availableTreeViewItem.Items.Refresh();
            }
        }

        /// <summary>
        /// Creates the treeview item for the given item.
        /// If this item is an object and contains additional items within the properties,
        /// these subitems are also creates as TreeViewItemss
        /// </summary>
        /// <param name="item">Item to be converted to a treeview</param>
        /// <param name="isRoot">true, if this is the root element. This means that the element is expanded. </param>
        /// <returns>The created element</returns>
        private TreeViewItem? CreateTreeViewItem(object item, bool isRoot = false)
        {
            if (_alreadyVisited.Contains(item))
            {
                return null;
            }

            _alreadyVisited.Add(item);

            // Perform filtering of the item
            if (FilterMetaClasses != null && item is IElement itemAsElement)
            {
                var list = FilterMetaClasses.ToList();
                if (list.Count != 0)
                {
                    var found = false;
                    foreach (var metaClass in list)
                    {
                        if (ClassifierMethods.IsSpecializedClassifierOf(
                            itemAsElement.metaclass,
                            metaClass))
                        {
                            found = true;
                        }
                    }

                    if (found == false)
                    {
                        return null;
                    }
                }
            }

            var itemHeader = GetItemHeader(item);

            var treeViewItem = new TreeViewItem
            {
                Header = itemHeader,
                Tag = item,
                IsExpanded = isRoot,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Top
            };

            if (_previouslySelectedItem?.Equals(item) == true)
            {
                _newSelectedItem = treeViewItem;
            }

            if (item is IExtent extent)
            {
                _mappingItems[extent] = treeViewItem;
                var childModels = new List<TreeViewItem>();
                var n = 0;
                foreach (var element in GetChildrenOfItem(extent))
                {
                    if (element == null) continue; // Skip the empty ones
                    
                    var created = CreateTreeViewItem(element);
                    if (created != null)
                        childModels.Add(created);
                    n++;
                    if (n >= MaxItemsPerLevel) break;
                }

                treeViewItem.ItemsSource = childModels;
            }
            else if (item is IObject itemAsObject)
            {
                _mappingItems[itemAsObject] = treeViewItem;

                var n = 0;

                // Gets the properties
                var childModels = new List<TreeViewItem>();
                foreach (var propertyValue in GetChildrenOfItem(itemAsObject))
                {
                    if (propertyValue is IReflectiveCollection childItems)
                    {
                        // If, we have a collection of properties, add the enumeration of the properties
                        // to the treeview (as long as we don't have too many items)
                        // Do not perform a resolving of the items
                        foreach (var childItem in CollectionHelper.EnumerateWithNoResolving(childItems, true))
                        {
                            if (childItem == null) continue;
                            var childTreeViewItem = CreateTreeViewItem(childItem);
                            if (childTreeViewItem != null)
                            {
                                childModels.Add(childTreeViewItem);
                            }

                            n++;

                            if (n >= MaxItemsPerLevel) break;
                        }
                    }
                    else if (propertyValue is IElement element)
                    {
                        var childTreeViewItem = CreateTreeViewItem(element);
                        if (childTreeViewItem != null)
                        {
                            childModels.Add(childTreeViewItem);
                        }

                        n++;
                    }

                    if (n >= MaxItemsPerLevel) break;
                }

                treeViewItem.ItemsSource = childModels;
            }

            return treeViewItem;
        }

        /// <summary>
        /// Gets the children of the items as they would be listed in the tree.
        /// This methods parsed extents and objects. 
        /// </summary>
        /// <param name="item">Item whose children are evaluated</param>
        /// <returns>Enumeration of children</returns>
        public IEnumerable<object?> GetChildrenOfItem(IObject item)
        {
            if (item is IExtent extent)
            {
                return extent.elements();
            }

            var result = new List<object>();
            var propertiesForChildren =
                ShowAllChildren // Defines whether all children shall be shown
                    ? (item as IObjectAllProperties)?.getPropertiesBeingSet().ToList() ?? new List<string>()
                    : _defaultClassifierHints.GetPackagingPropertyNames(item);
            foreach (var property in propertiesForChildren)
            {
                // Goes through the properties
                var propertyValue = item.getOrDefault<object>(property, true);
                
                if (propertyValue is IReflectiveCollection childItems)
                {
                    // If, we have a collection of properties, add the enumeration of the properties
                    // to the treeview (as long as we don't have too many items)
                    // Do not perform a resolving of the items
                    foreach (var childItem in CollectionHelper.EnumerateWithNoResolving(childItems, true))
                    {
                        if (childItem == null) continue;
                        result.Add(childItem);
                    }
                }
                else if (propertyValue is IElement element)
                {
                    result.Add(propertyValue);
                }
            }

            return result;
        }

        private string GetItemHeader(object item)
        {
            string itemHeader;
            if (item is IExtent extent)
            {
                itemHeader = "Root";
                
                if (ExtentManager.GetProviderCapabilities(extent).IsTemporaryStorage)
                {
                    itemHeader += " [Non-Permanent]";
                }
                else if (ExtentManager.IsExtentModified(extent))
                {
                    itemHeader += "*";
                }
            }
            else
            {
                // Filtering agrees to the item, so it will be added
                itemHeader = item.ToString() ?? string.Empty;
            }

            if (_cacheShowMetaClasses)
            {
                if (item is IElement element)
                {
                    var metaClass = element.getMetaClass();
                    itemHeader += " [" + (metaClass != null ? metaClass.ToString() : "Unclassified") + "]";
                }
            }

            return itemHeader ?? string.Empty;
        }


        private void ItemsTreeView_OnInitialized(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void OnItemChosen(object? item)
        {
            if (item is IObject itemAsObject)
            {
                ItemChosen?.Invoke(this, new ItemEventArgs(itemAsObject));
            }
        }

        private void OnItemSelected(object? item)
        {
            if (item is IObject itemAsObject)
            {
                ItemSelected?.Invoke(this, new ItemEventArgs(itemAsObject));
            }
        }

        /// <summary>
        /// This event is called, when the user double clicks on an item
        /// </summary>
        public event EventHandler<ItemEventArgs>? ItemChosen;

        /// <summary>
        /// This event is called, when the user double clicks on an item
        /// </summary>
        public event EventHandler<ItemEventArgs>? ItemSelected;

        private void treeView_MouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            if (TreeView.SelectedItem is TreeViewItem treeViewItem)
            {
                OnItemChosen(treeViewItem.Tag);
            }
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TreeView.SelectedItem is TreeViewItem treeViewItem)
                {
                    OnItemChosen(treeViewItem.Tag);
                }
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem treeViewItem)
            {
                OnItemSelected(treeViewItem.Tag);
            }
        }

        private void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CopyTreeToClipboard_OnClick()
        {
            var result = new StringBuilder();

            var currentText = "";

            _alreadyVisited.Clear();

            if (ItemsSource is IExtent extent)
            {
                VisitCopyTreeToClipboard(extent.elements(), currentText, result);
                Clipboard.SetText(result.ToString());
            }
            else
            {
                MessageBox.Show("The Root Object is not of type extent. The element could not be copied.");
            }
        }

        private void VisitCopyTreeToClipboard(IReflectiveCollection items, string currentText, StringBuilder result)
        {
            foreach (var item in items)
            {
                if (item == null || !_alreadyVisited.Contains(item)) continue;
                _alreadyVisited.Add(item);

                var itemAsObject = item as IObject;
                var myName = currentText + NamedElementMethods.GetName(itemAsObject);
                result.AppendLine(myName);

                if (itemAsObject == null)
                {
                    continue;
                }

                foreach (var property in _propertiesForChildren)
                {
                    var childItems = itemAsObject.getOrDefault<IReflectiveCollection>(property);
                    if (childItems != null)
                    {
                        VisitCopyTreeToClipboard(
                            childItems,
                            myName + "::",
                            result);
                    }
                }
            }
        }

        private void TreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ViewExtensions.OfType<TreeViewItemCommandDefinition>().All(x => x.Text != "Copy Tree to Clipboard"))
            {
                // Add default view extension
                ViewExtensions.Add(new TreeViewItemCommandDefinition(
                    "Copy Tree to Clipboard",
                    _ => CopyTreeToClipboard_OnClick()
                ));
            }
        }

        public INavigationHost NavigationHost
        {
            get => _navigationHost ?? throw new InvalidOperationException("NavigationHost == null");
            set => _navigationHost = value;
        }

        public IEnumerable<ViewExtension> GetViewExtensions()
            => Array.Empty<ViewExtension>();

        public void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions)
        {
            ViewExtensions.Clear();
            ViewExtensions.AddRange(viewExtensions.OfType<TreeViewItemCommandDefinition>());
        }

        /// <summary>
        /// Shows the context menu
        /// </summary>
        public void ShowContextMenu()
        {
            var menuItems = new List<MenuItem>();
            var selectedItem = GetSelectedItem();

            foreach (var extension in ViewExtensions.OfType<TreeViewItemCommandDefinition>())
            {
                if (extension.FilterFunction != null && !extension.FilterFunction(selectedItem))
                {
                    // Skip item
                    continue;
                }

                var menuItem = MenuHelper.GetOrCreateMenu(menuItems, extension.CategoryName, extension.Text);

                if (extension.Action == null)
                {
                    continue;
                }

                menuItem.Click += (x, y) =>
                {
                    extension.Action?.Invoke(selectedItem);
                };
            }

            ItemContextMenu.ItemsSource = menuItems;
            ItemContextMenu.IsOpen = true;
        }

        private void ItemContextMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            ShowContextMenu();
        }
    }
}