﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemsTreeView.xaml
    /// </summary>
    public partial class ItemsTreeView : UserControl
    {
        private IReflectiveCollection _itemsSource;

        private readonly HashSet<object> _alreadyVisited = new HashSet<object>();

        public static readonly DependencyProperty ShowRootProperty = DependencyProperty.Register(
            "ShowRoot", typeof(bool), typeof(ItemsTreeView), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a value indicating whether a root element shall be introduced
        /// </summary>
        public bool ShowRoot
        {
            get => (bool) GetValue(ShowRootProperty);
            set => SetValue(ShowRootProperty, value);
        }

        /// <summary>
        /// Defines the maximum items per level. This is used to reduce the amount of items
        /// </summary>
        private const int MaxItemsPerLevel = 100;

        /// <summary>
        /// Stores the properties being used to retrieve the items
        /// </summary>
        private readonly HashSet<string> _propertiesForChildren = new HashSet<string>();

        private Dictionary<IObject, TreeViewItem> _mappingItems = new Dictionary<IObject, TreeViewItem>();

        public ItemsTreeView()
        {
            InitializeComponent();
        }

        public IReflectiveCollection ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                UpdateView();
            }
        }

        public IObject SelectedElement
        {
            get
            {
                if (TreeView.SelectedItem is TreeViewItem treeViewItem)
                {
                    return treeViewItem.Tag as IObject;
                }

                return null;
            }
            set
            {
                if (_mappingItems.TryGetValue(value, out var treeviewItem))
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
            UpdateView();
        }

        /// <summary>
        /// Updates the complete view of the item tree
        /// </summary>
        private void UpdateView()
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

            var container = TreeView as ItemsControl;
            if (ShowRoot)
            {
                var rootItem = new TreeViewItem
                {
                    Header = "Root",
                    Tag = null,
                    IsExpanded = true
                };

                container.ItemsSource = new[] {rootItem};
                container = rootItem;
            }
            
            lock (_alreadyVisited)
            {
                var n = 0;
                _alreadyVisited.Clear();
                _mappingItems.Clear();
                foreach (var item in ItemsSource)
                {
                    var treeViewItem = CreateTreeViewItem(item);
                    if (treeViewItem != null)
                    {
                        model.Add(treeViewItem);
                        n++;
                    }

                    if (n >= MaxItemsPerLevel)
                    {
                        break;
                    }
                }

                container.ItemsSource = model;
            }
        }


        /// <summary>
        /// Creates the treeview item for the given item. 
        /// If thie item is an object and contains additional items within the properties, 
        /// these subitems are also creates as TreeViewItemss
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TreeViewItem CreateTreeViewItem(object item)
        {
            if (_alreadyVisited.Contains(item))
            {
                return null;
            }

            _alreadyVisited.Add(item);

            var treeViewItem = new TreeViewItem
            {
                Header = item.ToString(),
                Tag = item
            };

            if (item is IObject itemAsObject)
            {
                _mappingItems[itemAsObject] = treeViewItem;

                var n = 0;
                var childModels = new List<TreeViewItem>();
                foreach (var property in _propertiesForChildren)
                {
                    if (itemAsObject.getOrDefault(property) is IReflectiveCollection childItems)
                    {
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
        public event EventHandler<ItemEventArgs> ItemChosen;

        /// <summary>
        /// This event is called, when the user double clicks on an item 
        /// </summary>
        public event EventHandler<ItemEventArgs> ItemSelected;

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

        private void CopyTreeToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            var result = new StringBuilder();

            var currentText = "";
            var items = _itemsSource;

            _alreadyVisited.Clear();
            VisitCopyTreeToClipboard(items, currentText, result);

            Clipboard.SetText(result.ToString());
        }

        private void VisitCopyTreeToClipboard(IReflectiveCollection items, string currentText, StringBuilder result)
        {
            foreach (var item in items)
            {
                if (_alreadyVisited.Contains(item))
                {
                    continue;
                }

                _alreadyVisited.Add(item);

                var itemAsObject = item as IObject;
                var myName = currentText + UmlNameResolution.GetName(itemAsObject);
                result.AppendLine(myName);

                if (itemAsObject == null)
                {
                    continue;
                }

                foreach (var property in _propertiesForChildren)
                {
                    if (itemAsObject.getOrDefault(property) is IReflectiveCollection childItems)
                    {
                        VisitCopyTreeToClipboard(
                            childItems, 
                            myName + ".",
                            result);
                    }
                }
            }
        }
    }
}