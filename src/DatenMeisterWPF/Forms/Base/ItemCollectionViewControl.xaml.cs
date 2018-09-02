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

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemCollectionViewControl.xaml
    /// </summary>
    public partial class ItemCollectionViewControl : UserControl, INavigationGuest
    {
        public ItemCollectionViewControl()
        {
            InitializeComponent();
        }

        public INavigationHost NavigationHost { get; set; }
        public void PrepareNavigation()
        {
            throw new NotImplementedException();
        }
    }
}
