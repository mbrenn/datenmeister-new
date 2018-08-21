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

        /// <summary>
        /// Gets the currently selected element and copies it to the clipboard
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var selectedItem = listViewControl.GetSelectedItem();
            var builder = new StringBuilder();
            
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

            Clipboard.SetText(builder.ToString());
        }

        public event EventHandler CanExecuteChanged;
    }
}