﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
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
                if (treeView.SelectedItem is TreeViewItem treeViewItem)
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
                treeView.ItemsSource = null;
                return;
            }

            var model = new List<TreeViewItem>();
            
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

                treeView.ItemsSource = model;
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
            if (item is IObject element)
            {
                ItemChosen?.Invoke(this, new ItemEventArgs(element));
            }
        }

        /// <summary>
        /// This event is called, when the user double clicks on an item 
        /// </summary>
        public event EventHandler<ItemEventArgs> ItemChosen;

        private void treeView_MouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            if (treeView.SelectedItem is TreeViewItem treeViewItem)
            {
                OnItemChosen(treeViewItem.Tag);
            }
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (treeView.SelectedItem is TreeViewItem treeViewItem)
                {
                    OnItemChosen(treeViewItem.Tag);
                }
            }
        }
    }
}