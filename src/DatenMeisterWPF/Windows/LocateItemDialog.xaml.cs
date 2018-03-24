using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für LocateItemDialog.xaml
    /// </summary>
    public partial class LocateItemDialog : Window
    {
        private bool _asToolBox;

        /// <summary>
        /// Gets or sets the property, whether the window shall be shown as a toolbox
        /// or as a dialog. 
        /// If the window is set as a toolbox, it will not close upon request of 
        /// user and it will automatically create a detail window in case of selection.
        /// </summary>
        public bool AsToolBox
        {
            get => _asToolBox;
            set
            {
                _asToolBox = value;
                if (_asToolBox)
                {
                    WindowStyle = WindowStyle.ToolWindow;
                }
            }
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

        public LocateItemDialog()
        {
            InitializeComponent();
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            AcceptAndCloseDialog();
        }

        private void AcceptAndCloseDialog()
        {
            DialogResult = true;
            SelectedElement = items.SelectedElement;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Items_OnItemChosen(object sender, ItemEventArgs e)
        {
            if (e.Item != null)
            {
                if (AsToolBox)
                {
                    if (!(Owner is INavigationHost navigationHost))
                    {
                        throw new InvalidOperationException("Owner is not set or ist not a navigation host");
                    }

                    NavigatorForItems.NavigateToElementDetailView(
                        navigationHost,
                        e.Item);
                }
                else
                {
                    AcceptAndCloseDialog();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWorkspaces();
            UpdateExtents();
        }
    }
}
