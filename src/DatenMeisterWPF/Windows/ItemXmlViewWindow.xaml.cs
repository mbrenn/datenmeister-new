using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ItemXmlViewWindow.xaml
    /// </summary>
    public partial class ItemXmlViewWindow : Window
    {
        private IElement _item;

        public ItemXmlViewWindow()
        {
            InitializeComponent();
        }

        public IElement Item
        {
            get => _item;
            set
            {
                _item = value;
                UpdateContent();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateContent()
        {
            if (Item == null)
            {
                XmlTextField.Text = string.Empty;
            }
            else
            {
                var xmiConverter = new XmlConverter();
                var element = xmiConverter.ConvertToXml(Item);
                XmlTextField.Text = element.ToString();
            }
        }
    }
}
