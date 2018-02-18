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
using System.Windows.Shapes;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
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

        public LocateItemDialog()
        {
            InitializeComponent();
        }

        public IWorkspaceLogic WorkspaceLogic { get; set; }

        /// <summary>
        /// Stores the default extent being used by the user
        /// </summary>
        private IExtent _defaultExtent;

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

        private void Items_OnItemDoubleClicked(object sender, ItemEventArgs e)
        {
            if (e.Item != null)
            {
                AcceptAndCloseDialog();
            }
        }
    }
}
