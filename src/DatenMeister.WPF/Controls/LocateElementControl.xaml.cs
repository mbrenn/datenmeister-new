﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration.DotNet;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für LocateElement.xaml
    /// </summary>
    public partial class LocateElementControl : UserControl
    {
        public LocateElementControl()
        {
            InitializeComponent();

            _workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWorkspaces();
            UpdateExtents();
        }
        
        /// <summary>
        /// Defines the metaclasses for the filter
        /// </summary>
        private IList<IElement> _metaClassesForFilter = new List<IElement>();

        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the selected workspace used by the user
        /// </summary>
        private IWorkspace? _selectedWorkspace;

        /// <summary>
        /// Stores the default extent being used by the user
        /// </summary>
        private IExtent? _selectedExtent;

        /// <summary>
        /// Gets the selected element
        /// </summary>
        public IObject? SelectedElement => items.GetSelectedItem();

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(LocateElementControl),
            new PropertyMetadata(default(bool),
                OnIsReadOnlyChanged));

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LocateElementControl locateElementControl))
            {
                throw new InvalidOperationException("Dependency object is not of type ItemsTreeView");
            }

            var isReadOnly = (bool) e.NewValue;

            locateElementControl.cboExtent.IsEnabled = !isReadOnly;
            locateElementControl.cboWorkspace.IsEnabled = !isReadOnly;
            locateElementControl.items.IsEnabled = !isReadOnly;
        }

        /// <summary>
        /// Gets or sets a flag indicating whether the control shall be read only ==> the user cannot
        /// modify the selected item
        /// </summary>
        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty ShowWorkspaceSelectionProperty =
            DependencyProperty.Register(
                "ShowWorkspaceSelection",
                typeof(bool),
                typeof(LocateElementControl),
                new PropertyMetadata(
                    true,
                    OnShowWorkSpaceSelectionChanged));

        public bool ShowWorkspaceSelection
        {
            get => (bool) GetValue(ShowWorkspaceSelectionProperty);
            set => SetValue(ShowWorkspaceSelectionProperty, value);
        }

        public static readonly DependencyProperty ShowMetaClassesProperty = DependencyProperty.Register(
            "ShowMetaClasses", typeof(bool), typeof(LocateElementControl),
            new PropertyMetadata(default(bool), OnShowMetaClassesChange));

        private static void OnShowMetaClassesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LocateElementControl locateElementControl))
            {
                throw new InvalidOperationException("Dependency object is not of type ItemsTreeView");
            }
            
            locateElementControl.items.ShowMetaClasses = (bool) e.NewValue;
        }
    
        public bool ShowMetaClasses
        {
            get => (bool) GetValue(ShowMetaClassesProperty);
            set => SetValue(ShowMetaClassesProperty, value);
        }

        private static void OnShowWorkSpaceSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LocateElementControl) d;
            var newValue = (bool) e.NewValue;
            control.txtWorkspace.Visibility =
                control.cboWorkspace.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public static readonly DependencyProperty ShowExtentSelectionProperty = DependencyProperty.Register(
            "ShowExtentSelection",
            typeof(bool),
            typeof(LocateElementControl),
            new PropertyMetadata(
                true,
                OnShowExtentSelectionChanged));

        private static void OnShowExtentSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LocateElementControl)d;
            var newValue = (bool)e.NewValue;
            control.txtExtent.Visibility =
                control.cboExtent.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool ShowExtentSelection
        {
            get => (bool) GetValue(ShowExtentSelectionProperty);
            set => SetValue(ShowExtentSelectionProperty, value);
        }

        public static readonly DependencyProperty ShowAllChildrenProperty = DependencyProperty.Register(
            "ShowAllChildren", typeof(bool), typeof(LocateElementControl),
            new PropertyMetadata(default(bool)));

        public bool ShowAllChildren
        {
            get => (bool) GetValue(ShowAllChildrenProperty);
            set
            {
                SetValue(ShowAllChildrenProperty, value);
                items.ShowAllChildren = true;
            }
        }

        /// <summary>
        /// Gets or sets the metaclasses that will be filtered
        /// </summary>
        public IEnumerable<IElement>? FilterMetaClasses
        {
            get => items.FilterMetaClasses;
            set => items.FilterMetaClasses = value;
        }

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        private IWorkspace? SelectedWorkspace
        {
            get => _selectedWorkspace;
            set
            {
                _selectedWorkspace = value;
                if (cboWorkspace.ItemsSource == null)
                {
                    return;
                }

                foreach (var cboItem in cboWorkspace.ItemsSource
                    .Cast<object>()
                    .Where(cboItem => (cboItem as ComboBoxItem)?.Tag == value))
                {
                    cboExtent.SelectedItem = cboItem;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        private IExtent? SelectedExtent
        {
            get => _selectedExtent;
            set
            {
                _selectedExtent = value;
                if (cboExtent.ItemsSource == null)
                {
                    return;
                }

                foreach (var cboItem in cboExtent.ItemsSource
                    .Cast<object>()
                    .Where(cboItem => (cboItem as ComboBoxItem)?.Tag == value))
                {
                    cboExtent.SelectedItem = cboItem;
                    break;
                }
            }
        }

        /// <summary>
        /// Navigates to a specific workspace
        /// </summary>
        /// <param name="workspace">Workspace to be shown</param>
        public void Select(IWorkspace? workspace)
        {
            SelectedWorkspace = workspace;
            SelectedExtent = null;
        }

        /// <summary>
        /// Selects a specific extent as a predefined one
        /// </summary>
        /// <param name="workspace">Workspace to be shown</param>
        /// <param name="extent">Extent to be selected</param>
        public void Select(IWorkspace? workspace, IExtent? extent)
        {
            SelectedWorkspace = workspace;
            SelectedExtent = extent;
        }

        /// <summary>
        /// Selects a specific extent as a predefined one
        /// </summary>
        /// <param name="extent">Extent to be selected</param>
        public void Select(IExtent? extent)
        {
            if (extent == null)
            {
                SelectedWorkspace = null;
                SelectedExtent = null;
            }
            else
            {
                var workspace = _workspaceLogic.FindWorkspace(extent);
                SelectedWorkspace = workspace;
                SelectedExtent = extent;
            }
        }

        public void Select(IObject value)
        {
            var extent = value.GetExtentOf();
            Select(extent);
            
            items.SetSelectedItem(value);
        }
        
        /// <summary>
        /// Sets the meta classes for filters
        /// </summary>
        /// <param name="elements">Elements to be filed</param>
        public void SetMetaClassesForFilter(IList<IElement> elements)
        {
            _metaClassesForFilter = elements;
        }

        /// <summary>
        /// Updates the items for the workspace
        /// </summary>
        private void UpdateWorkspaces()
        {
            var workspaces = _workspaceLogic.Workspaces;
            cboExtent.ItemsSource = null;

            var comboWorkspaces = new List<object>();

            var index = -1;
            var n = 0;
            foreach (var workspace in workspaces)
            {
                var cboItem = new ComboBoxItem
                {
                    Content = workspace.id,
                    Tag = workspace
                };

                if (workspace == SelectedWorkspace)
                {
                    index = n;
                }

                comboWorkspaces.Add(cboItem);
                n++;
            }

            // Sets the selected workspace
            cboWorkspace.ItemsSource = comboWorkspaces;
            if (index != -1)
            {
                cboWorkspace.SelectedIndex = index;
            }
            else
            {
                cboWorkspace.SelectedItem = null;
            }
        }

        /// <summary>
        /// Updates the items for all extents
        /// </summary>
        private void UpdateExtents()
        {
            if (_selectedWorkspace == null)
            {
                cboExtent.IsEnabled = false;
                cboExtent.SelectedItem = null;
            }
            else
            {
                cboExtent.IsEnabled = true;

                var comboItems = new List<object>();
                var index = -1;
                var n = 0;
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
                        index = n;
                    }

                    n++;
                }

                cboExtent.ItemsSource = comboItems;

                // Sets the selected workspace
                if (index != -1)
                {
                    cboExtent.SelectedIndex = index;
                }
                else
                {
                    cboExtent.SelectedItem = null;
                }
            }
        }

        private void cboWorkspace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedWorkspace = (cboWorkspace.SelectedItem as ComboBoxItem)?.Tag as IWorkspace;
            UpdateExtents();
        }

        private void cboExtents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            items.SetDefaultProperties();
            if (cboExtent.SelectedItem is ComboBoxItem selected)
            {
                switch (selected.Tag)
                {
                    case IWorkspace _:
                        items.ItemsSource = null;
                        items.TreeView.IsEnabled = false;
                        break;
                    case IExtent extent:
                        items.FilterMetaClasses =
                            _metaClassesForFilter.Any()
                                ? DefaultClassifierHints.GetDefaultPackageClassifiers(extent)
                                    .Union(_metaClassesForFilter)
                                : null;
                        items.ItemsSource = extent;
                        items.TreeView.IsEnabled = true;
                        break;
                    default:
                        items.ItemsSource = null;
                        items.TreeView.IsEnabled = false;
                        break;
                }
            }
            else
            {
                items.ItemsSource = null;
                items.TreeView.IsEnabled = false;
            }
        }

        public void SelectWorkspace(string workspaceId)
        {
            var workspace = (IWorkspace?) _workspaceLogic.GetWorkspace(workspaceId);
            if (workspace != null)
                Select(workspace);
        }

        public void SelectExtent(string extentUri)
        {
            if (SelectedWorkspace != null)
            {
                Select(SelectedWorkspace.FindExtent(extentUri));
            }
        }

        /// <summary>
        /// Sets the given reflection as the root objects.
        /// </summary>
        /// <param name="value">Value to be shown.</param>
        /// <param name="showOnlyObject">true, if the workspace and extent options are hidden and cannot be selected by the user</param>
        public void SetAsRoot(IObject value, bool showOnlyObject = true)
        {
            ShowWorkspaceSelection = !showOnlyObject;
            ShowExtentSelection = !showOnlyObject;

            items.ItemsSource = value;
        }
    }
}