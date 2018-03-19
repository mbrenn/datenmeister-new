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
        public IObject SelectedElement { get; private set; }

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

        public LocateItemDialog()
        {
            InitializeComponent();
        }

        public IWorkspaceLogic WorkspaceLogic { get; set; }

        /// <summary>
        /// Stores the default extent being used by the user
        /// </summary>
        private IExtent _defaultExtent;

        private bool _asToolBox;

        /// <summary>
        /// Gets or sets the default extent which is preselected
        /// </summary>
        public IExtent DefaultExtent
        {
            get => _defaultExtent;
            set
            {
                _defaultExtent = value;
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

        public void UpdatedWorkspaces()
        {
            var workspaces = WorkspaceLogic.Workspaces;
            cboExtents.ItemsSource = null;

            var comboItems = new List<object>();

            foreach (var workspace in workspaces)
            {
                var cboItem = new ComboBoxItem
                {
                    Content = workspace.id,
                    Tag = workspace
                };

                comboItems.Add(cboItem);

                foreach (var extent in workspace.extent.OfType<IUriExtent>())
                {
                    var cboExtentItem = new ComboBoxItem
                    {
                        Content = $"- {extent.contextURI()}",
                        Tag = extent
                    };

                    comboItems.Add(cboExtentItem);

                    if (extent == _defaultExtent)
                    {
                        cboExtents.SelectedItem = cboExtentItem;
                    }
                }
            }

            cboExtents.ItemsSource = comboItems;
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
                        break;
                    case IExtent extent:
                        items.ItemsSource = extent.elements();
                        break;
                    default:
                        items.ItemsSource = null;
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
            UpdatedWorkspaces();
        }
    }
}
