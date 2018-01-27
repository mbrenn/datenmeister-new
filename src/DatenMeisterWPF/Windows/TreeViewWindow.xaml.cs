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

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für TreeViewWindow.xaml
    /// </summary>
    public partial class TreeViewWindow : Window
    {
        public TreeViewWindow()
        {
            InitializeComponent();
        }

        public void SetCollection(IReflectiveCollection collection)
        {
            treeView.ItemsSource = collection;
        }

        public void AddPropertyForChild(params string[] properties)
        {
            treeView.AddPropertyForChild(properties);
        }

        /// <summary>
        /// Sets the default properties for the view.
        /// The default property is "packagedElement" as child package for Packages
        /// </summary>
        public void SetDefaultProperties()
        {
            treeView.SetDefaultProperties();
        }
    }
}
