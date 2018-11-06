using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Command
{
    public class CopyToClipboardCommand : ICommand
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(CopyToClipboardCommand));

        private IHasSelectedItems listViewControl;

        public CopyToClipboardCommand(IHasSelectedItems listViewControl)
        {
            this.listViewControl = listViewControl;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void OnExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Gets the currently selected element and copies it to the clipboard
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var selectedItems = listViewControl.GetSelectedItems();
            if (selectedItems == null)
            {
                selectedItems = new[] {listViewControl.GetSelectedItem()};
            }

            selectedItems = selectedItems.ToList();

            var first = true;
            var builder = new StringBuilder();
            var selectedList = selectedItems.ToList();
            foreach (var selectedItem in selectedList)
            {
                if (!first)
                {
                    builder.AppendLine();
                    builder.AppendLine("------");
                    builder.AppendLine();
                }

                if (!(selectedItem is IObjectAllProperties allProperties))
                {

                    return;
                }

                foreach (var property in allProperties.getPropertiesBeingSet())
                {
                    var value = DotNetHelper.AsString(
                        selectedItem.getOrDefault(property));

                    builder.AppendLine($"{property}: {value}");
                }

                first = false;
            }

            var xmiProvider = new XmiProvider();
            var tempExtent = new MofExtent(xmiProvider);
            string xmlResult;
            if (selectedList.Count == 1)
            {
                var result = ObjectCopier.Copy(new MofFactory(tempExtent), selectedList.First());
                xmlResult = ((XmiProviderObject)((MofObject) result).ProviderObject).XmlNode.ToString();
            }
            else
            {
                var extentCopier = new ExtentCopier(new MofFactory(tempExtent));
                extentCopier.Copy(selectedList, tempExtent.elements());
                xmlResult = xmiProvider.Document.ToString();
            }

            var dataObject = new DataObject();
            dataObject.SetText(builder.ToString());
            dataObject.SetData("XMI", xmlResult);

            try
            {
                Clipboard.SetDataObject(dataObject);
            }
            catch (Exception exc)
            {
                Logger.Error($"Copy to clipboard failed: {exc}");
                MessageBox.Show($"Copy to clipboard failed: {exc}");
            }
        }

        /// <summary>
        /// Tritt ein, wenn Änderungen auftreten, die sich auf die Ausführung des Befehls auswirken.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}