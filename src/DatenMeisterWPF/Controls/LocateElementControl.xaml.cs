using System;
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
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
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

            _workspaceLogic = App.Scope?.Resolve<IWorkspaceLogic>();
            if (_workspaceLogic != null)
            {
                UpdateWorkspaces();
                UpdateExtents();
            }
        }

        private IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the selected workspace used by the user
        /// </summary>
        private IWorkspace _selectedWorkspace;

        /// <summary>
        /// Stores the default extent being used by the user
        /// </summary>
        private IExtent _selectedExtent;

        public IObject SelectedElement
        {
            get => items.SelectedElement;
        }

        public static readonly DependencyProperty ShowWorkspaceSelectionProperty = DependencyProperty.Register(
            "ShowWorkspaceSelection", typeof(bool), typeof(LocateElementControl), new PropertyMetadata(default(bool)));

        public bool ShowWorkspaceSelection
        {
            get => (bool) GetValue(ShowWorkspaceSelectionProperty);
            set => SetValue(ShowWorkspaceSelectionProperty, value);
        }

        public static readonly DependencyProperty ShowExtentSelectionProperty = DependencyProperty.Register(
            "ShowExtentSelection", typeof(bool), typeof(LocateElementControl), new PropertyMetadata(default(bool)));

        public bool ShowExtentSelection
        {
            get => (bool) GetValue(ShowExtentSelectionProperty);
            set => SetValue(ShowExtentSelectionProperty, value);
        }

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        private IWorkspace SelectedWorkspace
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
        private IExtent SelectedExtent
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
        public void Select(IWorkspace workspace)
        {
            SelectedWorkspace = workspace;
        }

        /// <summary>
        /// Selects a specific extent as a predefined one
        /// </summary>
        /// <param name="workspace">Workspace to be shown</param>
        /// <param name="extent">Extent to be selected</param>
        public void Select(IWorkspace workspace, IExtent extent)
        {
            SelectedWorkspace = workspace;
            SelectedExtent = extent;
        }

        /// <summary>
        /// Selects a specific extent as a predefined one
        /// </summary>
        /// <param name="extent">Extent to be selected</param>
        public void Select(IExtent extent)
        {
            var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
            var workspace = workspaceLogic.FindWorkspace(extent);
            SelectedWorkspace = workspace;
            SelectedExtent = extent;
        }

        public void Select(IObject value)
        {
            var extent = value.GetExtentOf();
            Select(extent);
            items.SelectedElement = value;
        }

        /// <summary>
        /// Updates the items for the workspace
        /// </summary>
        private void UpdateWorkspaces()
        {
            var workspaces = _workspaceLogic.Workspaces;
            cboExtents.ItemsSource = null;

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
                cboExtents.IsEnabled = false;
                cboExtents.SelectedItem = null;
            }
            else
            {
                cboExtents.IsEnabled = true;

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

                cboExtents.ItemsSource = comboItems;

                // Sets the selected workspace
                if (index != -1)
                {
                    cboExtents.SelectedIndex = index;
                }
                else
                {
                    cboExtents.SelectedItem = null;
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
            if (cboExtents.SelectedItem is ComboBoxItem selected)
            {
                switch (selected.Tag)
                {
                    case IWorkspace _:
                        items.ItemsSource = null;
                        items.TreeView.IsEnabled = false;
                        break;
                    case IExtent extent:
                        items.ItemsSource = extent.elements();
                        items.TreeView.IsEnabled = true;
                        break;
                    default:
                        items.ItemsSource = null;
                        items.TreeView.IsEnabled = false;
                        break;
                }
            }
        }
    }
}
