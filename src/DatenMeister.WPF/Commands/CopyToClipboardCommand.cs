using System.Text;
using System.Windows;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Commands
{
    /// <summary>
    ///     The enumeration defines the format of the copying
    /// </summary>
    public enum CopyType
    {
        /// <summary>
        ///     Copies as text
        /// </summary>
        Default,

        /// <summary>
        ///     Copies as text
        /// </summary>
        AsText,

        /// <summary>
        ///     Copies as Xmi
        /// </summary>
        AsXmi
    }

    public static class CopyToClipboardCommand
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(CopyToClipboardCommand));

        /// <summary>
        ///     Gets the currently selected element and copies it to the clipboard
        /// </summary>
        /// <param name="hasSelectedItems">The selected items</param>
        /// <param name="copyType">The type of the format to be copied to clipboard</param>
        public static void Execute(IHasSelectedItems hasSelectedItems, CopyType copyType)
        {
            // Checks, whether we can retrieve the selected item directly or if we need to use the IHasSelectedItems interface
            var selectedItems = 
                hasSelectedItems.GetSelectedItems().ToArray();

            if (!selectedItems.Any())
            {
                var selectedItem = hasSelectedItems.GetSelectedItem();
                if (selectedItem != null)
                {
                    selectedItems = new[] {selectedItem};
                }
                else
                {
                    // No selection, nothing to do
                    return;
                }
            }
            
            Execute(selectedItems, copyType);
        }

        /// <summary>
        ///     Gets the currently selected element and copies it to the clipboard
        /// </summary>
        /// <param name="item">The selected item</param>
        /// <param name="copyType">The type of the format to be copied to clipboard</param>
        public static void Execute(IObject item, CopyType copyType)
        {
            // Checks, whether we can retrieve the selected item directly or if we need to use the IHasSelectedItems interface
            Execute(new[] {item}, copyType);
        }

        public static void Execute(IEnumerable<IObject> selectedItems, CopyType copyType)
        {
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

                if (!(selectedItem is IObjectAllProperties allProperties)) return;

                foreach (var property in allProperties.getPropertiesBeingSet())
                {
                    var value = selectedItem.getOrDefault<string>(property);

                    builder.AppendLine($"{property}: {value}");
                }

                first = false;
            }

            var xmiProvider = new XmiProvider();
            var tempExtent = new MofExtent(xmiProvider);
            string xmlResult;
            if (selectedList.Count == 1)
            {
                var copier = new ObjectCopier(new MofFactory(tempExtent));
                var result = copier.Copy(selectedList.First(), new CopyOption {CloneAllReferences = false});
                xmlResult = ((XmiProviderObject) ((MofObject) result).ProviderObject).XmlNode.ToString();
            }
            else
            {
                var extentCopier = new ExtentCopier(new MofFactory(tempExtent));
                extentCopier.Copy(selectedList, tempExtent.elements());
                xmlResult = xmiProvider.Document.ToString();
            }

            try
            {
                switch (copyType)
                {
                    case CopyType.Default:
                        var dataObject = new DataObject();
                        dataObject.SetText(builder.ToString());
                        dataObject.SetData("XMI", xmlResult);
                        Clipboard.SetDataObject(dataObject);
                        break;
                    case CopyType.AsText:
                        Clipboard.SetText(builder.ToString());
                        break;
                    case CopyType.AsXmi:
                        Clipboard.SetText(xmlResult);
                        break;
                    default:
                        goto case CopyType.Default;
                }
            }
            catch (Exception exc)
            {
                Logger.Error($"Copy to clipboard failed: {exc}");
                MessageBox.Show($"Copy to clipboard failed: {exc}");
            }
        }
    }
}