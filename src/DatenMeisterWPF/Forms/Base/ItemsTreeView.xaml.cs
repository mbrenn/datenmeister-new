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
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemsTreeView.xaml
    /// </summary>
    public partial class ItemsTreeView : UserControl
    {
        private IReflectiveCollection _itemsSource;

        /// <summary>
        /// Stores the properties being used to retrieve the items
        /// </summary>
        private List<string> _propertiesForChildren = new List<string>();

        public ItemsTreeView()
        {
            InitializeComponent();
        }

        public IReflectiveCollection ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                UpdateView();
            }
        }

        public void AddPropertyForChild(params string[] properties)
        {
            _propertiesForChildren.AddRange(properties);
        }


        private void UpdateView()
        { 
            if (!IsInitialized || ItemsSource == null)
            {
                // Save the time... 
                return;
            }

            foreach (var item in ItemsSource)
            {
                var treeViewItem = new TreeViewItem();

                treeViewItem.Header = item.ToString();

                if (item is IObject itemAsObject)
                {
                    foreach (var property in _propertiesForChildren)
                    {
                        if (itemAsObject.isSet(property))
                        {
                            var childItems = itemAsObject.get(property) as IReflectiveCollection;
                        }
                    }
                }


            }
            throw new NotImplementedException();
        }

        private void ItemsTreeView_OnInitialized(object sender, EventArgs e)
        {
            UpdateView();
        }
    }
}
