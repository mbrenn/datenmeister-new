using System;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
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


        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
            "MessageText", typeof(string), typeof(LocateItemDialog), new PropertyMetadata(default(string), OnMessageTextChanged));

        public string MessageText
        {
            get => (string)GetValue(MessageTextProperty);
            set => SetValue(MessageTextProperty, value);
        }


        private static void OnMessageTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LocateItemDialog)d).txtTitle.Text = (string)e.NewValue;
        }

        public LocateItemDialog()
        {
            InitializeComponent();
        }

        public bool ShowWorkspaceSelection
        {
            get => LocateElementControl.ShowWorkspaceSelection;
            set => LocateElementControl.ShowWorkspaceSelection = value;
        }
        public bool ShowExtentSelection
        {
            get => LocateElementControl.ShowExtentSelection;
            set => LocateElementControl.ShowExtentSelection = value;
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

        public void SelectWorkspace(string workspaceId)
        {
            LocateElementControl.SelectWorkspace(workspaceId);
        }

        public void SelectExtent(string extentUri)
        {
            LocateElementControl.SelectExtent(extentUri);
        }

        public void SetAsRoot(IReflectiveCollection package)
        {
            LocateElementControl.SetAsRoot(package);
        }
    }
}
