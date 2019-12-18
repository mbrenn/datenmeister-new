#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.TreeView;
using DatenMeister.WPF.Navigation;
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
        private IObject? _itemsSource;

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
            
            itemsTreeView.UpdateView();
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
                UpdateView();
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
                    UpdateView();
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
                UpdateView();
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

        public ItemsTreeView()
        {
            InitializeComponent();
        }

        public IObject? ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                UpdateView();
            }
        }

        public IObject? SelectedElement
        {
            get
            {
                if (TreeView.SelectedItem is TreeViewItem treeViewItem)
                {
                    return treeViewItem.Tag as IObject;
                }

                return ItemsSource;
            }
            set
            {
                if (value != null && _mappingItems.TryGetValue(value, out var treeviewItem))
                {
                    treeviewItem.IsSelected = true;
                }
            }
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

            UpdateView();
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
        public void UpdateView()
        {
            if (!IsInitialized)
            {
                // Save the time...
                return;
            }

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
                var found = CreateTreeViewItem(ItemsSource, true);
                if (found != null)
                    model.Add(found);

                container.ItemsSource = model;
            }

            if (_newSelectedItem != null)
            {
                _newSelectedItem.IsSelected = true;
                _newSelectedItem.IsExpanded = true;
                _newSelectedItem.BringIntoView();
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
                            itemAsElement.metaclass
                            , metaClass))
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

            // Filtering agrees to the item, so it will be added
            var itemHeader = item is IExtent ? "Root" : item.ToString();
            if (_cacheShowMetaClasses)
            {
                if (item is IElement element)
                {
                    var metaClass = element.getMetaClass();
                    itemHeader += " [" + (metaClass != null ? metaClass.ToString() : "Unclassified") + "]";
                }
            }
            
            var treeViewItem = new TreeViewItem
            {
                Header = itemHeader,
                Tag = item,
                IsExpanded = isRoot
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
                foreach (var element in extent.elements())
                {
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
                var propertiesForChildren =
                    ShowAllChildren
                        ? (item as IObjectAllProperties)?.getPropertiesBeingSet().ToList() ?? new List<string>()
                        : _propertiesForChildren.ToList();

                foreach (var property in propertiesForChildren)
                {
                    // Goes through the properties
                    var propertyValue = itemAsObject.GetOrDefault(property);
                    if (propertyValue is IReflectiveCollection childItems)
                    {
                        // If, we have a collection of properties, add the enumeration of the properties
                        // to the treeview (as long as we don't have too many items)
                        foreach (var childItem in childItems)
                        {
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


        private void ItemsTreeView_OnInitialized(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void OnItemChosen(object item)
        {
            ItemChosen?.Invoke(this, new ItemEventArgs(item as IObject));
        }

        private void OnItemSelected(object item)
        {
            ItemSelected?.Invoke(this, new ItemEventArgs(item as IObject));
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
            foreach (var item in
                items.Where(item => !_alreadyVisited.Contains(item)))
            {
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
                    if (itemAsObject.GetOrDefault(property) is IReflectiveCollection childItems)
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

        public INavigationHost? NavigationHost { get; set; }

        public IEnumerable<ViewExtension> GetViewExtensions()
            => Array.Empty<ViewExtension>();

        public void EvaluateViewExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            var menuItems = new List<MenuItem>();
            foreach (var extension in ViewExtensions.OfType<TreeViewItemCommandDefinition>())
            {
                var menuItem = new MenuItem
                {
                    Header = extension.Text
                };

                if (extension.Action == null)
                {
                    continue;
                }
                
                
                menuItem.Click += (x, y) =>
                {
                    var selectedItem = SelectedElement;
                    if (selectedItem == null)
                    {
                        System.Windows.MessageBox.Show("No item selected");
                    }

                    extension.Action?.Invoke(selectedItem);
                };
                
                menuItems.Add(menuItem);
            }

            ItemContextMenu.ItemsSource = menuItems;
        }
    }
}