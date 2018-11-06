using System;
using System.Windows;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.Copier;

namespace DatenMeisterWPF.Command
{
    public class PasteToClipboardCommand
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(PasteToClipboardCommand));
        private readonly IObject _targetElement;

        public PasteToClipboardCommand(IObject targetElement)
        {
            _targetElement = targetElement;
        }

        /// <summary>
        ///     Gets the currently selected element and copies it to the clipboard
        /// </summary>
        public void Execute()
        {
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null)
            {
                MessageBox.Show("Clipboard empty");
                return;
            }

            var dataAsXmi = dataObject.GetData("XMI")?.ToString();
            if (dataAsXmi == null)
            {
                MessageBox.Show("No XMI");
                return;
            }

            try
            {
                var document = XDocument.Parse(dataAsXmi);
                if (document.Root?.Name == "item") // Just one item
                {
                    if (_targetElement != null)
                    {
                        var xmiProvider = new XmiProvider();
                        var tempExtent = new MofExtent(xmiProvider);

                        var providerObject = new XmiProviderObject(document.Root, xmiProvider);
                        var element = new MofElement(providerObject, tempExtent);

                        var copier = new ObjectCopier(new MofFactory(tempExtent));
                        copier.CopyProperties(element, _targetElement);
                    }
                }
                else
                {
                    Logger.Info($"Unknown document type: {document.Root?.Name}");
                    MessageBox.Show($"Unknown document type: {document.Root?.Name}");
                }

                MessageBox.Show(document.ToString());
            }
            catch (Exception exc)
            {
                Logger.Error($"Paste from clipboard failed: {exc}");
                MessageBox.Show($"Paste from clipboard failed: {exc}");
            }
        }
    }
}