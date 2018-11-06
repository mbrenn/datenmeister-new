﻿using System;
using System.Linq;
using System.Text;
using System.Windows;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Command
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

    public class CopyToClipboardCommand
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(CopyToClipboardCommand));

        private readonly IHasSelectedItems listViewControl;

        public CopyToClipboardCommand(IHasSelectedItems listViewControl)
        {
            this.listViewControl = listViewControl;
        }

        /// <summary>
        ///     Gets the currently selected element and copies it to the clipboard
        /// </summary>
        /// <param name="copyType">The type of the format to be copied to clipboard</param>
        public void Execute(CopyType copyType)
        {
            var selectedItems = listViewControl.GetSelectedItems();
            if (selectedItems == null) selectedItems = new[] {listViewControl.GetSelectedItem()};

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