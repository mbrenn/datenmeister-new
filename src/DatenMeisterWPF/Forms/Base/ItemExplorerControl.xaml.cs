﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemBrowser.xaml
    /// </summary>
    public partial class ItemExplorerControl : UserControl, INavigationGuest
    {
        /// <summary>
        /// Stores the information about the active tab controls
        /// </summary>
        protected ObservableCollection<ItemExplorerTab> Tabs = new ObservableCollection<ItemExplorerTab>();

        /// <summary>
        /// Gets or sets the items to be shown. These items are shown also in the navigation view and will
        /// not be modified, even if the user clicks on the navigation tree. 
        /// </summary>
        protected IReflectiveCollection Items { get; set; }

        /// <summary>
        /// Defines the item that the user currently has selected ont the object tree
        /// </summary>
        protected IObject SelectedPackage{ get; set; }

        /// <summary>
        /// Gets or sets the items to be shown in the detail view. Usually, they are the same as the items.
        /// If the user clicks on the navigation tree, a subview of the items may be shown
        /// </summary>
        protected IReflectiveCollection DetailItems { get; set; }

        public ItemExplorerControl()
        {
            InitializeComponent();
            ItemTabControl.ItemsSource = Tabs;
        }

        public INavigationHost NavigationHost { get; set; }

        public void PrepareNavigation()
        {
        }

        /// <summary>
        /// Updates the tree content of the explorer view
        /// </summary>
        private void UpdateTreeContent()
        {
            NavigationTreeView.SetDefaultProperties();
            NavigationTreeView.ItemsSource = Items;
        }

        public virtual void OnMouseDoubleClick(IObject element)
        {
            NavigateToElement(element);
        }

        /// <summary>
        /// Adds a new tab to the form
        /// </summary>
        /// <param name="collection">Collection being used</param>
        /// <param name="viewDefinition">Form to be added</param>
        public ItemExplorerTab AddTab(IReflectiveCollection collection, ViewDefinition viewDefinition)
        {
            var control = new ItemListViewControl();
            control.NavigationHost = NavigationHost;

            // Gets the default view for the given tab
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            IElement result = null;

            switch (viewDefinition.Mode)
            {
                // Used, when an external function requires a specific view mode
                case ViewDefinitionMode.Specific:
                    result = viewDefinition.Element;
                    break;
                // Creates the view by creating the 'all Properties' view by parsing all the items
                case ViewDefinitionMode.AllProperties:
                    result = viewFinder.CreateView(Items);
                    break;
                case ViewDefinitionMode.Default:
                    break;
            }
            
            if (result == null)
            {
                // Nothing was found... so, create your default list lsit. 
                result = viewFinder.CreateView(Items);
            }

            control.CurrentFormDefinition = result;
            var tabControl = new ItemExplorerTab
            {
                Content = control,
                Header = UmlNameResolution.GetName(result)
            };

            control.SetContent(collection);
            Tabs.Add(tabControl);

            // Selects the item, if none of the items are selected
            if (ItemTabControl.SelectedItem == null)
            {
                ItemTabControl.SelectedItem = tabControl;
            }

            
            return tabControl;
        }

        private void NavigationTreeView_OnItemChosen(object sender, ItemEventArgs e)
        {
            NavigateToElement(e.Item);
        }

        private void NavigationTreeView_OnItemSelected(object sender, ItemEventArgs e)
        {
            SelectedPackage = e.Item;
            if (e.Item != null)
            {
                DetailItems = new PropertiesAsReflectiveCollection(e.Item);
                UpdateContent();
            }
            else
            {
                // When user has selected the root element or no other item, all items are shown
                DetailItems = Items;
                UpdateContent();
            }
        }

        /// <summary>
        /// Updates the content of all sub elements
        /// </summary>
        public void UpdateContent()
        {
            UpdateTreeContent();

            foreach (var tab in Tabs)
            {
                tab.Control.UpdateContent();
            }
        }

        /// <summary>
        /// Opens the selected element
        /// </summary>
        /// <param name="selectedElement">Selected element</param>
        private void NavigateToElement(IObject selectedElement)
        {
            if (selectedElement == null)
            {
                return;
            }

            var events = NavigatorForItems.NavigateToElementDetailView(NavigationHost, selectedElement as IElement);
            events.Closed += (sender, args) => UpdateContent();
        }

        /// <summary>
        /// Clears the infolines
        /// </summary>
        public void ClearInfoLines()
        {
            InfoLines.Children.Clear();
        }

        /// <summary>
        /// Adds an infoline to the window
        /// </summary>
        /// <param name="element">Element to be added</param>
        public void AddInfoLine(UIElement element)
        {
            InfoLines.Children.Add(element);
        }
    }
}
