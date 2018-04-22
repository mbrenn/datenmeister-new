﻿using System;
using System.Collections.Generic;
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
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Controls
{
    /// <summary>
    /// Interaktionslogik für LocateElement.xaml
    /// </summary>
    public partial class LocateElementControl : UserControl
    {
        public LocateElementControl()
        {
            InitializeComponent();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWorkspaces();
            UpdateExtents();
        }

        public IWorkspaceLogic WorkspaceLogic { get; set; }

        /// <summary>
        /// Stores the selected workspace used by the user
        /// </summary>
        private IWorkspace _selectedWorkspace;

        /// <summary>
        /// Stores the default extent being used by the user
        /// </summary>
        private IExtent _selectedExtent;

        public IObject SelectedElement { get; private set; }

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        protected IWorkspace SelectedWorkspace
        {
            get => _selectedWorkspace;
            set
            {
                _selectedWorkspace = value;
                if (cboWorkspace.ItemsSource == null)
                {
                    return;
                }

                foreach (var cboItem in cboWorkspace.ItemsSource)
                {
                    if ((cboItem as ComboBoxItem)?.Tag == value)
                    {
                        cboExtents.SelectedItem = cboItem;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        protected IExtent SelectedExtent
        {
            get => _selectedExtent;
            set
            {
                _selectedExtent = value;
                if (cboExtents.ItemsSource == null)
                {
                    return;
                }

                foreach (var cboItem in cboExtents.ItemsSource)
                {
                    if ((cboItem as ComboBoxItem)?.Tag == value)
                    {
                        cboExtents.SelectedItem = cboItem;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Navigates to a specific workspace
        /// </summary>
        /// <param name="workspace">Workspace to be shown</param>
        public void NavigateToWorkspace(IWorkspace workspace)
        {
            SelectedWorkspace = workspace;
        }

        /// <summary>
        /// Navigates to a specific extent
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="extent"></param>
        public void NavigateToExtent(IWorkspace workspace, IExtent extent)
        {
            SelectedWorkspace = workspace;
            SelectedExtent = extent;
        }

        /// <summary>
        /// Updates the items for the workspace
        /// </summary>
        private void UpdateWorkspaces()
        {
            var workspaces = WorkspaceLogic.Workspaces;
            cboExtents.ItemsSource = null;

            var comboWorkspaces = new List<object>();

            foreach (var workspace in workspaces)
            {
                var cboItem = new ComboBoxItem
                {
                    Content = workspace.id,
                    Tag = workspace
                };

                comboWorkspaces.Add(cboItem);
            }

            cboWorkspace.ItemsSource = comboWorkspaces;
        }

        /// <summary>
        /// Updates the items for all extents
        /// </summary>
        private void UpdateExtents()
        {
            if (_selectedWorkspace == null)
            {
                cboExtents.IsEnabled = false;
            }
            else
            {
                cboExtents.IsEnabled = true;

                var comboItems = new List<object>();
                foreach (var extent in _selectedWorkspace.extent.OfType<IUriExtent>())
                {
                    var cboExtentItem = new ComboBoxItem
                    {
                        Content = extent.contextURI(),
                        Tag = extent
                    };

                    comboItems.Add(cboExtentItem);

                    if (extent == _selectedExtent)
                    {
                        cboExtents.SelectedItem = cboExtentItem;
                    }
                }

                cboExtents.ItemsSource = comboItems;
            }

            cboExtents.SelectedItem = null;
        }

        private void cboWorkspace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedWorkspace = (cboWorkspace.SelectedItem as ComboBoxItem)?.Tag as IWorkspace;
            UpdateExtents();
        }

        private void cboExtents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            items.SetDefaultProperties();
            if (cboExtents.SelectedItem is ComboBoxItem selected)
            {
                switch (selected.Tag)
                {
                    case IWorkspace _:
                        items.ItemsSource = null;
                        items.treeView.IsEnabled = false;
                        break;
                    case IExtent extent:
                        items.ItemsSource = extent.elements();
                        items.treeView.IsEnabled = true;
                        break;
                    default:
                        items.ItemsSource = null;
                        items.treeView.IsEnabled = false;
                        break;
                }
            }
        }
    }
}
