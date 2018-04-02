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

        public IObject SelectedElement { get; set; }

        public LocateItemDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigates to a specific workspace
        /// </summary>
        /// <param name="workspace">Workspace to be shown</param>
        public void Select(IWorkspace workspace)
        {
            LocateElementControl.Select(workspace);
        }

        /// <summary>
        /// Navigates to a specific extent
        /// </summary>
        /// <param name="extent"></param>
        public void Select(IExtent extent)
        {
            LocateElementControl.Select(extent);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            AcceptAndCloseDialog();
        }

        private void AcceptAndCloseDialog()
        {
            DialogResult = true;
            SelectedElement = LocateElementControl.SelectedElement;
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
    }
}
