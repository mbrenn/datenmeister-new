using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Extensions;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace DatenMeister.WPF.Forms.Base;

/// <summary>
/// Interaktionslogik für ItemsTreeView.xaml
/// </summary>
public partial class ItemsTreeView : UserControl, INavigationGuest
{
    /// <summary>
    /// Just a configuration option which may be set during the development to recreate all items in treeview
    /// instead of reusing the existing items
    /// </summary>
    private const bool ConfigurationAlwaysRefresh = false; // TODO: true does not work with ShowTypeAsPackage
            
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
    private readonly HashSet<object> _alreadyVisited = new();

    public static readonly DependencyProperty ShowRootProperty = DependencyProperty.Register(
        "ShowRoot", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool)));

    public static readonly DependencyProperty ShowMetaClassesProperty = DependencyProperty.Register(
        "ShowMetaClasses", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool), OnShowMetaClassesChange));
        
    public static readonly DependencyProperty ShowTypeAsPackageProperty = DependencyProperty.Register(
        "ShowTypeAsPackage", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(true, OnShowTypeAsPackageChange));

    private static void OnShowMetaClassesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is ItemsTreeView itemsTreeView))
        {
            throw new InvalidOperationException("Dependency object is not of type ItemsTreeView");
        }

        var newValue = (bool) e.NewValue;
        itemsTreeView.UpdateForm();
        itemsTreeView._cacheShowMetaClasses = newValue;

        if (newValue != (itemsTreeView.ShowMetaClassesCheckBtn.IsChecked == true))
        {
            itemsTreeView.ShowMetaClassesCheckBtn.IsChecked = newValue;
        }
    }

    public bool ShowMetaClasses
    {
        get => (bool) GetValue(ShowMetaClassesProperty);
        set => SetValue(ShowMetaClassesProperty, value);
    }

    public bool ShowTypeAsPackage
    {
        get => (bool) GetValue(ShowTypeAsPackageProperty);
        set => SetValue(ShowTypeAsPackageProperty, value);
    }
        
    private static void OnShowTypeAsPackageChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is ItemsTreeView itemsTreeView))
        {
            throw new InvalidOperationException("Dependency object is not of type ItemsTreeView");
        }

        var newValue = (bool) e.NewValue;
        itemsTreeView.UpdateForm();

        if (newValue != (itemsTreeView.ShowMetaClassesCheckBtn.IsChecked == true))
        {
            itemsTreeView.TypeAsPackageButton.IsChecked = newValue;
        }
    }

    private bool _cacheShowMetaClasses;

    /// <summary>
    /// Gets a list of the viewextension for the tree view
    /// </summary>
    public List<ViewExtension> ViewExtensions { get; } = new();

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
    /// Determines whether the filter for the metaclasses are enabled. 
    /// </summary>
    private bool _enableFilterMetaClasses = true;

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
    private readonly HashSet<string> _propertiesForChildren = new();

    /// <summary>
    /// Maps the items to the treeviewitem
    /// </summary>
    private readonly Dictionary<IObject, TreeViewItem> _mappingItems = new();

    /// <summary>
    /// Stores the previously selected item when the tree is rebuilt.
    /// This allows the refocussing to the item
    /// </summary>
    private object? _previouslySelectedItem;

    /// <summary>
    /// Stores the item that shall be selected, after the items have been created
    /// </summary>
    private TreeViewItem? _newSelectedItem;
        
    private INavigationHost? _navigationHost;

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
        if (TreeView.SelectedItem is ItemsTreeViewItem treeViewItem
            && treeViewItem.TreeViewItemParameter != null)
        {
            return treeViewItem.TreeViewItemParameter.Element;
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

    public void ClearForm()
    {
        TreeView.ItemsSource = null;
    }

    /// <summary>
    /// Updates the form and of all including buttons
    /// </summary>
    public void UpdateForm()
    {
        UpdateForm(true);
    }

    /// <summary>
    /// Updates the complete view of the item tree
    /// </summary>
    /// <param name="updateButtons">true, if all the buttons shall be updated</param>
    public void UpdateForm(bool updateButtons)
    {
        if (!IsInitialized)
        {
            // Save the time...
            return;
        }

        if (updateButtons)
        {
            UpdateFilterMetaClassButton();
        }

        using var watch = new StopWatchLogger(Logger, "UpdateView", LogLevel.Trace);

        if (ItemsSource == null)
        {
            TreeView.ItemsSource = null;
            return;
        }

        var model = new List<ItemsTreeViewItem>();

        _newSelectedItem = null;
        _previouslySelectedItem =
            (TreeView.SelectedItem as ItemsTreeViewItem)?.TreeViewItemParameter?.Element;

        var container = TreeView as ItemsControl;
        lock (_alreadyVisited)
        {
            _alreadyVisited.Clear();
            _mappingItems.Clear();

            // Checks, if a tree views are already created
            var availableTreeViewItem = (container.ItemsSource as List<ItemsTreeViewItem>)?.FirstOrDefault();

            // ReSharper disable once RedundantLogicalConditionalExpressionOperand
            if (availableTreeViewItem == null || ConfigurationAlwaysRefresh)
            {
                var found = CreateTreeViewItemLazy(
                    new TreeViewItemParameter(ItemsSource),
                    true);
                    
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
    private void UpdateTreeViewItem(ItemsTreeViewItem availableTreeViewItem, IObject item)
    {
        // Updates the current item;
        if (availableTreeViewItem.TreeViewItemParameter != null)
        {
            availableTreeViewItem.Header = GetItemHeader(availableTreeViewItem.TreeViewItemParameter);
            availableTreeViewItem.TreeViewItemParameter.Element = item;
        }

        var childrenOfItem = GetChildrenOfItem(item).ToList();
            
        if (availableTreeViewItem.ItemsSource is List<ItemsTreeViewItem> viewChildren)
        {
            foreach (var viewChild in viewChildren.ToList())
            {
                if (viewChild.TreeViewItemParameter == null)
                {
                    // If the parameter are not set, remove them
                    // Should not happen, but who knows
                    viewChildren.Remove(viewChild);
                    continue;
                }
                    
                // Checks, if the given element is in the children of Item
                // ReSharper disable once RedundantLogicalConditionalExpressionOperand
                var found = childrenOfItem
                    .FirstOrDefault(
                        x => viewChild.TreeViewItemParameter.Element?.equals(x.Element) == true);
                    
                // ReSharper disable once RedundantLogicalConditionalExpressionOperand
                if (found?.Element is { } foundItem && !ConfigurationAlwaysRefresh)
                {
                    // If the element was found, add it
                    UpdateTreeViewItem(viewChild, foundItem);
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
                if (child.Element is null)
                    continue;

                var createdTreeViewItem = CreateTreeViewItemLazy(child);
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
    /// these subitems are also creates as TreeViewItems
    /// </summary>
    /// <param name="item">Item to be converted to a treeview</param>
    /// <param name="isRoot">true, if this is the root element. This means that the element is expanded. </param>
    /// <returns>The created element</returns>
    private ItemsTreeViewItem? CreateTreeViewItemLazy(TreeViewItemParameter item, bool isRoot = false)
    {
        if (item.Element == null || _alreadyVisited.Contains(item.Element))
        {
            return null;
        }

        _alreadyVisited.Add(item.Element);

        // Perform filtering of the item
        if (_enableFilterMetaClasses && FilterMetaClasses != null && item.Element is IElement itemAsElement)
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

        var parameter =
            new TreeViewItemParameter(
                item.Element,
                item.ParentElement,
                item.ParentProperty
            );

        var itemHeader = GetItemHeader(parameter);

        var treeViewItem = new ItemsTreeViewItem
        {
            Header = itemHeader,
            TreeViewItemParameter = parameter,
            IsExpanded = isRoot,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Top
        };

        if (_previouslySelectedItem?.Equals(item) == true)
        {
            _newSelectedItem = treeViewItem;
        }
            
        _mappingItems[item.Element] = treeViewItem;

        if (!isRoot)
        {
            treeViewItem.Items.Add("Loading...");
        }
        else
        {
            CreateSubTreeItems(treeViewItem, parameter);
        }

        return treeViewItem;
    }

    /// <summary>
    /// Creates the subtree upon the elements being child of the current treeViewItem.
    /// This function is used for lazy loading
    /// </summary>
    /// <param name="treeViewItem">The WPF Treeview item being showed</param>
    /// <param name="itemParameter">The parameter of the current item whose children shall
    /// be listed. </param>
    private void CreateSubTreeItems(ItemsControl treeViewItem, TreeViewItemParameter itemParameter)
    {
        if (itemParameter.Element == null) return;

        treeViewItem.Items.Clear();
        var parentItem = treeViewItem;

        if (!ShowTypeAsPackage)
        {
            var childItems =
                GetChildrenOfItem(itemParameter.Element)
                    .Take(MaxItemsPerLevel);
            parentItem.ItemsSource = CreateSubTree(childItems);
        }
        else
        {
            // Group the elements to 
            var groupModels = new List<TreeViewItem>();
            var childItems =
                GetChildrenOfItem(itemParameter.Element);
            var groupedItems =
                ByMetaClassGrouper.Group(
                        childItems,
                        x => x.Element)
                    .OrderBy(x => NamedElementMethods.GetName(x.MetaClass))
                    .ToList();

            if (groupedItems.Count == 1)
            {
                parentItem.ItemsSource =
                    CreateSubTree(groupedItems.First().Elements.Take(MaxItemsPerLevel));
            }
            else
            {
                groupModels.AddRange(
                    groupedItems.Select(
                        group => 
                            new TreeViewItem
                            {
                                Header = $"[{NamedElementMethods.GetName(group.MetaClass)}]", 
                                ItemsSource = CreateSubTree(group.Elements.Take(MaxItemsPerLevel)),
                            }));

                parentItem.ItemsSource = groupModels;
            }
        }

        List<ItemsTreeViewItem> CreateSubTree(IEnumerable<TreeViewItemParameter> treeViewItemParameters)
        {
            var childModels = new List<ItemsTreeViewItem>();

            // Gets the properties
            foreach (var propertyValue in treeViewItemParameters)
            {
                var childTreeViewItem = CreateTreeViewItemLazy(propertyValue);
                if (childTreeViewItem != null)
                {
                    childModels.Add(childTreeViewItem);
                }

                childModels.Sort((x, y) =>
                    NamedElementMethods.GetName(x)?.CompareTo(NamedElementMethods.GetName(y)) ?? 0);
            }

            return childModels;
        }
    }

    /// <summary>
    /// Gets the children of the items as they would be listed in the tree.
    /// This methods parsed extents and objects. 
    /// </summary>
    /// <param name="item">Item whose children are evaluated</param>
    /// <returns>Enumeration of children</returns>
    public IEnumerable<TreeViewItemParameter> GetChildrenOfItem(IObject item)
    {
        if (item is IExtent extent)
        {
            return extent.elements()
                .OfType<IObject>()
                .Take(MaxItemsPerLevel)
                .Select(x => new TreeViewItemParameter(x, extent));
        }

        var result = new List<TreeViewItemParameter>();
        var propertiesForChildren =
            ShowAllChildren // Defines whether all children shall be shown
                ? (item as IObjectAllProperties)?.getPropertiesBeingSet().ToList() ?? new List<string>()
                : DefaultClassifierHints.GetPackagingPropertyNames(item);
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
                    if (!(childItem is IObject asObject)) continue;
                    result.Add(new TreeViewItemParameter(asObject, item, property));

                    if (result.Count > MaxItemsPerLevel)
                    {
                        break;
                    }
                }
            }
            else if (propertyValue is IElement asElement)
            {
                result.Add(new TreeViewItemParameter(asElement, item, property));
            }
                
            if (result.Count > MaxItemsPerLevel)
            {
                break;
            }
        }

        return result;
    }

    private void TreeView_OnExpanded(object sender, RoutedEventArgs e)
    {
        if (!(e.OriginalSource is ItemsTreeViewItem item)) return;
            
        if (item.Items.Count == 1 && item.Items[0] is string && item.TreeViewItemParameter != null)
        {
            CreateSubTreeItems(item, item.TreeViewItemParameter);
        }
    }

    private string GetItemHeader(TreeViewItemParameter parameter)
    {
        var item = parameter.Element;
        string itemHeader;
        if (item is IExtent extent)
        {
            itemHeader = "Root";
                
            if (ExtentManager.GetProviderCapabilities(extent).IsTemporaryStorage)
            {
                itemHeader += " [Non-Permanent]";
            }
                
            if (ExtentManager.IsExtentModified(extent))
            {
                itemHeader += "*";
            }

            if (_cacheShowMetaClasses)
            {
                var configuration = extent.GetConfiguration();
                if (!string.IsNullOrEmpty(configuration.ExtentType))
                {
                    itemHeader += $" {{{configuration.ExtentType}}}";
                }
            }
        }
        else
        {
            // Filtering agrees to the item, so it will be added
            itemHeader = item?.ToString() ?? string.Empty;
        }

        if (_cacheShowMetaClasses && item is IElement element)
        {
            var metaClass = element.getMetaClass();
            itemHeader += " [" + (metaClass != null ? metaClass.ToString() : "Unclassified") + "]";
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
        if (TreeView.SelectedItem is ItemsTreeViewItem treeViewItem 
            && treeViewItem.TreeViewItemParameter != null)
        {
            OnItemChosen(treeViewItem.TreeViewItemParameter.Element);
        }
    }

    private void treeView_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (TreeView.SelectedItem is ItemsTreeViewItem treeViewItem
                && treeViewItem.TreeViewItemParameter != null)
            {
                OnItemChosen(treeViewItem.TreeViewItemParameter.Element);
            }
        }
    }

    private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is ItemsTreeViewItem treeViewItem
            && treeViewItem.TreeViewItemParameter != null)
        {
            OnItemSelected(treeViewItem.TreeViewItemParameter.Element);
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

    private void VisitCopyTreeToClipboard(IEnumerable items, string currentText, StringBuilder result)
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
                if (!(TreeView.SelectedItem is ItemsTreeViewItem selectedTreeItem)
                    || selectedTreeItem.TreeViewItemParameter == null)
                {
                    MessageBox.Show("Somehow, no treeitem is selected");
                    return;
                }

                extension.Action?.Invoke(selectedTreeItem.TreeViewItemParameter);
            };
        }

        ItemContextMenu.ItemsSource = menuItems;
        ItemContextMenu.IsOpen = true;
    }

    private void ItemContextMenu_OnOpened(object sender, RoutedEventArgs e)
    {
        ShowContextMenu();
    }

    private void UpdateFilterMetaClassButton()
    {
        if (_filterMetaClasses == null || _filterMetaClasses?.Count() == 0)
        {
            FilterMetaClassCheck.IsEnabled = false;
            return;
        }

        FilterMetaClassCheck.IsEnabled = true;
        FilterMetaClassCheck.IsChecked = _enableFilterMetaClasses;
    }

    private void ShowMetaClassesCheckBtn_Click(object sender, RoutedEventArgs e)
    {
        ShowMetaClasses = ShowMetaClassesCheckBtn.IsChecked == true;
        UpdateForm();
    }

    private void FilterMetaClassCheck_Click(object sender, RoutedEventArgs e)
    {
        _enableFilterMetaClasses =
            FilterMetaClassCheck.IsChecked == true;

        // Clear Complete Form
        ClearForm();
        UpdateForm(false);
    }

    private void TypeAsPackage_Click(object sender, RoutedEventArgs e)
    {
        ShowTypeAsPackage = 
            TypeAsPackageButton.IsChecked == true;

        // Clear Complete Form
        ClearForm();
        UpdateForm(false);
    }
}