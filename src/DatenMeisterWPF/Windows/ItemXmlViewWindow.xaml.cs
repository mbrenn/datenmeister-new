using System.Collections.Generic;
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
        public ItemXmlViewWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Sets the content of the window by converting the given elements to an xml
        /// </summary>
        /// <param name="elements">Items to be shown</param>
        public void UpdateContent(IEnumerable<IObject> elements)
        {
            var xmiConverter = new XmlConverter();
            var element = xmiConverter.ConvertToXml(elements);
            XmlTextField.Text = element.ToString();
        }

        /// <summary>
        /// Sets the content of the window to convert the element to an xml
        /// </summary>
        /// <param name="item">Item to be shown</param>
        public void UpdateContent(IElement item)
        {
            if (item == null)
            {
                XmlTextField.Text = string.Empty;
            }
            else
            {
                var xmiConverter = new XmlConverter();
                var element = xmiConverter.ConvertToXml(item);
                XmlTextField.Text = element.ToString();
            }
        }
    }
}
