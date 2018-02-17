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

        public void UpdatedWorkspaces()
        {
            var workspaces = WorkspaceLogic.Workspaces;
            cboExtents.ItemsSource = null;

            var comboItems = new List<object>();

            foreach (var workspace in workspaces)
            {
                var cboIem = new ComboBoxItem
                {
                    Content = workspace.id,
                    Tag = workspace
                };

                comboItems.Add(cboIem);

                foreach (var extent in workspace.extent.OfType<IUriExtent>())
                {
                    var cboExtentIem = new ComboBoxItem
                    {
                        Content = $"- {extent.contextURI()}",
                        Tag = extent
                    };

                    comboItems.Add(cboExtentIem);
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
            DialogResult = true;
            SelectedElement = items.SelectedElement;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
