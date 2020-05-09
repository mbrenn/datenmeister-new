using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    /// <summary>
    /// Defines the detail tagging field
    /// </summary>
    public class CheckboxListTaggingField : IDetailField
    {
        /// <summary>
        /// Stores the lists of options
        /// </summary>
        private List<CheckBox> _options = new List<CheckBox>();

        public UIElement? CreateElement(IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            var valuePairs =
                fieldData.getOrDefault<IReflectiveCollection>(_FormAndFields._CheckboxListTaggingFieldData.options);

            _options = new List<CheckBox>();
            foreach (var pair in valuePairs.OfType<IElement>())
            {
                var name = pair.getOrDefault<string>(_FormAndFields._ValuePair.name);
                var valueContent = pair.getOrDefault<string>(_FormAndFields._ValuePair.value);

                var checkbox = new CheckBox
                {
                    Content = name, 
                    Tag = valueContent
                };

                _options.Add(checkbox);
            }

            // Adds the stack panel
            var stackPanel = new StackPanel {Orientation = Orientation.Vertical};
            foreach (var option in _options)
            {
                stackPanel.Children.Add(option);
            }

            return stackPanel;
        }

        public void CallSetAction(IObject element)
        {
        }
    }
}