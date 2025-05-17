#nullable enable

using System.Windows;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Provider.Xmi.Provider.XMI;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ItemXmlViewWindow.xaml
    /// </summary>
    public partial class ItemXmlViewWindow : Window
    {
        private IReflectiveCollection? _usedReflectiveCollection;

        private IObject? _usedObject;

        public bool SupportWriting
        {
            get => UpdateButton.Visibility == Visibility.Collapsed;
            set
            {
                UpdateButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                XmlTextField.IsReadOnly = !value;
            }
        }

        public bool IgnoreIDs
        {
            get => IgnoreIDsBtn.IsChecked == true;
            set => IgnoreIDsBtn.IsChecked = value;
        }

        /// <summary>
        /// Called, if the user clicks the update button
        /// </summary>
        public event EventHandler<EventArgs>? UpdateButtonPressed;

        public ItemXmlViewWindow()
        {
            InitializeComponent();
            SupportWriting = false;
            RelativePaths.IsChecked = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Sets the content of the window by converting the given elements to an xml
        /// </summary>
        /// <param name="elements">Items to be shown</param>
        public void UpdateContent(IReflectiveCollection elements)
        {
            _usedReflectiveCollection = elements;
            _usedObject = null;

            UpdateContent();
        }

        /// <summary>
        /// Gets the content as a DotNet Element
        /// </summary>
        /// <param name="factory">Factory being used to create the element</param>
        /// <returns>Element being retrieved</returns>
        public IElement GetCurrentContentAsMof(IFactory factory)
        {
            var xmlElement = XElement.Parse(XmlTextField.Text);
            
            var provider = new XmiProvider();
            var extent = new MofExtent(provider);
            var xmlProviderObject = provider.CreateProviderObject(xmlElement);
            return new MofElement(xmlProviderObject, extent);
        }

        /// <summary>
        /// Sets the content of the window to convert the element to an xml
        /// </summary>
        /// <param name="item">Item to be shown</param>
        public void UpdateContent(IObject item)
        {
            _usedReflectiveCollection = null;
            _usedObject = item;

            UpdateContent();
        }

        public void UpdateContent()
        {
            var converter = new XmlConverter
            {
                SkipIds = IgnoreIDsBtn.IsChecked == true,
                RelativePaths = RelativePaths.IsChecked == true
                
            };

            if (_usedReflectiveCollection != null)
            {
                var element = converter.ConvertToXml(_usedReflectiveCollection);
                XmlTextField.Text = element.ToString();
            }
            else if (_usedObject != null)
            {
                var element = converter.ConvertToXml(_usedObject);
                XmlTextField.Text = element.ToString();
            }
            else
            {
                XmlTextField.Text = string.Empty;
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

        /// <summary>
        /// Copies the content of the item to the clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            Clipboard.SetText(XmlTextField.Text);
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
        private void Ignore_IDs_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateContent();
        }
        private void RelativePaths_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateContent();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Owner?.Focus();
        }
    }
}
