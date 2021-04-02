#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
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

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IObject? SelectedElement { get; set; }


        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
            "MessageText", typeof(string), typeof(LocateItemDialog), new PropertyMetadata(default(string), OnMessageTextChanged));

        public static readonly DependencyProperty SelectButtonTextProperty = DependencyProperty.Register(
            "SelectButtonText", typeof(string), typeof(LocateItemDialog), new PropertyMetadata(default(string), OnSelectButtonTextChanged));
        
        public string MessageText
        {
            get => (string) GetValue(MessageTextProperty);
            set => SetValue(MessageTextProperty, value);
        }

        public void SetMetaClassesForFilter(IList<IElement> elements)
        {
            LocateElementControl.SetMetaClassesForFilter(elements);
        }

        /// <summary>
        /// Gets or sets the text of the Selection Button
        /// </summary>
        public string SelectButtonText
        {
            get => (string) GetValue(SelectButtonTextProperty);
            set => SetValue(SelectButtonTextProperty, value);
        }

        private static void OnMessageTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LocateItemDialog) d).txtTitle.Text = (string) e.NewValue;
        }

        private static void OnSelectButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LocateItemDialog) d).SelectionButton.Content = (string) e.NewValue;
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

        public string DescriptionText
        {
            get => txtDescription.Text;
            set => txtDescription.Text = value;
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
            SelectedElement = LocateElementControl.SelectedElement;
            if (_asToolBox)
            {
                if (SelectedElement != null)
                {
                    OnItemChosen(SelectedElement);
                }
            }
            else
                DialogResult = true;

            // Opens the dialog
            if (!(Owner is INavigationHost navigationHost))
                throw new InvalidOperationException("Owner is not set or ist not a navigation host");

            navigationHost.SetFocus();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (!_asToolBox)
                DialogResult = false;

            Close();
        }

        /// <summary>
        /// Defines the event that is called when the user has chosen an item
        /// </summary>
        public event EventHandler<ItemEventArgs>? ItemChosen;

        public void SelectWorkspace(string workspaceId)
        {
            LocateElementControl.SelectWorkspace(workspaceId);
        }

        public void SelectExtent(string extentUri)
        {
            LocateElementControl.SelectExtent(extentUri);
        }

        public void SetAsRoot(IObject element)
        {
            LocateElementControl.SetAsRoot(element);
        }

        protected virtual void OnItemChosen(IObject chosenElement)
        {
            ItemChosen?.Invoke(this, new ItemEventArgs(chosenElement));
        }

        private void LocateItemDialog_OnClosed(object sender, EventArgs e)
        {
            Owner?.Focus();
        }
    }
}
