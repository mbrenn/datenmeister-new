using System.Windows;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;

namespace DatenMeister.WPF.Commands;

public class PasteToClipboardCommand
{
    private static readonly ClassLogger Logger = new(typeof(PasteToClipboardCommand));
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
        try
        {
            // Gets the information by clipboard
            string? dataAsXmi;
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null)
            {
                dataAsXmi = Clipboard.GetText();
            }
            else
            {
                dataAsXmi =
                    dataObject.GetData("XMI")?.ToString() ?? dataObject.GetData(DataFormats.Text)?.ToString();
            }

            if (dataAsXmi == null)
            {
                MessageBox.Show("No XMI");
                return;
            }

            // Now parses the xml
            var document = XDocument.Parse(dataAsXmi);
            if (document.Root?.Name == "item") // Just one item
            {
                var xmiProvider = new XmiProvider();
                var tempExtent = new MofExtent(xmiProvider);

                var providerObject = xmiProvider.CreateProviderObject(document.Root);
                var element = new MofElement(providerObject, tempExtent);

                var copier = new ObjectCopier(new MofFactory(tempExtent));
                copier.CopyProperties(element, _targetElement);
            }
            else
            {
                Logger.Info($"Unknown document type: {document.Root?.Name}");
                MessageBox.Show($"Unknown document type: {document.Root?.Name}");
            }
        }
        catch (Exception exc)
        {
            Logger.Error($"Paste from clipboard failed: {exc}");
            MessageBox.Show($"Paste from clipboard failed: {exc}");
        }
    }
}