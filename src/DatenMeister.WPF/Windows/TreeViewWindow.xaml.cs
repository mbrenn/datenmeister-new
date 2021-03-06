﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für TreeViewWindow.xaml
    /// </summary>
    public partial class TreeViewWindow : Window, INavigationGuest
    {
        private INavigationHost? _navigationHost;

        /// <summary>
        /// Called, if the user selected and double clicked an item
        /// </summary>
        public event EventHandler<ItemEventArgs>? ItemSelected;

        /// <summary>
        /// Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost
        {
            get => _navigationHost ?? throw new InvalidOperationException("NavigationHost == null");
            set => _navigationHost = value;
        }

        public TreeViewWindow()
        {
            InitializeComponent();
        }

        public void SetRootItem(IObject value)
        {
            ObjectTreeView.ItemsSource = value;
        }

        public void AddPropertyForChild(params string[] properties)
        {
            ObjectTreeView.AddPropertyForChild(properties);
        }

        /// <summary>
        /// Sets the default properties for the view.
        /// The default property is "packagedElement" as child package for Packages
        /// </summary>
        public void SetDefaultProperties()
        {
            ObjectTreeView.SetDefaultProperties();
        }

        private void OnItemSelected(object? item)
        {
            if (item is IObject element)
            {
                ItemSelected?.Invoke(this, new ItemEventArgs(element));
            }
        }

        private void TreeView_OnItemSelected(object sender, ItemEventArgs e)
        {
            OnItemSelected(e.Item);
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation
        /// host.
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            return new ViewExtension[] { };
        }

        /// <summary>
        /// Evaluates the given view extensions.
        /// Currently, the TreeViewWindow does not support viewextensions
        /// </summary>
        /// <param name="viewExtensions">Viewextensions being evaluated</param>
        public void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions)
        {
            ObjectTreeView.EvaluateViewExtensions(viewExtensions);
        }

        public void UpdateForm()
        {
            ObjectTreeView.UpdateForm();
        }

        private void TreeViewWindow_OnClosed(object sender, EventArgs e)
        {
            Owner?.Focus();
        }
    }
}
