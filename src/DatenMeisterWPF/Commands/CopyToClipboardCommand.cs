using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Command
{
    public class CopyToClipboardCommand : ICommand
    {
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

            var first = true;
            var builder = new StringBuilder();
            foreach (var selectedItem in selectedItems)
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

            Clipboard.SetText(builder.ToString());
        }

        /// <summary>Tritt ein, wenn Änderungen auftreten, die sich auf die Ausführung des Befehls auswirken.</summary>
        public event EventHandler CanExecuteChanged;
    }
}