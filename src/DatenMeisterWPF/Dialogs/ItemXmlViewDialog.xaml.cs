using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ItemXmlViewWindow.xaml
    /// </summary>
    public partial class ItemXmlViewWindow : Window
    {
        public bool SupportWriting
        {
            get => UpdateButton.Visibility == Visibility.Collapsed;
            set
            {
                UpdateButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                XmlTextField.IsReadOnly = !value;
            }
        }

        /// <summary>
        /// Called, if the user clicks the update button
        /// </summary>
        public event EventHandler<EventArgs> UpdateButtonPressed;

        public ItemXmlViewWindow()
        {
            InitializeComponent();
            SupportWriting = false;
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
        /// Gets the content as a DotNet Element
        /// </summary>
        /// <param name="factory">Factory being used to create the element</param>
        /// <returns>Element being retrieved</returns>
        public IElement GetCurrentContentAsMof(IFactory factory)
        {
            var xmlElement = XElement.Parse(XmlTextField.Text);
            var xmlConverter = new XmlConverter();

            return (IElement) xmlConverter.ConvertFromXml(xmlElement, factory);
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

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Invalid content: \r\n\r\n" + exc);
            }
        }
        /*

        /// <summary>
        /// Event arguments being used when the user performs an update
        /// </summary>
        public class UpdateEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the element being used
            /// </summary>
            public IElement Element
            {
                get;
                set;
            }
        }*/
    }
}
