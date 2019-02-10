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
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für TreeViewWindow.xaml
    /// </summary>
    public partial class TreeViewWindow : Window, INavigationGuest
    {
        /// <summary>
        /// Called, if the user selected and double clicked an item
        /// </summary>
        public event EventHandler<ItemEventArgs> ItemSelected;

        /// <summary>
        /// Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        public TreeViewWindow()
        {
            InitializeComponent();
        }

        public void SetCollection(IReflectiveCollection collection)
        {
            ObjectTreeView.ItemsSource = collection;
        }

        public void AddPropertyForChild(params string[] properties)
        {
            ObjectTreeView.AddPropertyForChild(properties);
        }

        /// <summary>
        /// Sets the default properties for the view.
        /// The default property is "packagedElement" as child package for Packages
        /// </summary>
        public void SetDefaultProperties()
        {
            ObjectTreeView.SetDefaultProperties();
        }


        private void OnItemSelected(object item)
        {
            if (item is IObject element)
            {
                ItemSelected?.Invoke(this, new ItemEventArgs(element));
            }
        }

        private void TreeView_OnItemSelected(object sender, ItemEventArgs e)
        {
            OnItemSelected(e.Item);
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            return new ViewExtension[] { };
        }
    }
}
