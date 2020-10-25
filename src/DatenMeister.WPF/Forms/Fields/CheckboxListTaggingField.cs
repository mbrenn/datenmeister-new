using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
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
        /// Stores the name
        /// </summary>
        private string _name = string.Empty;

        private string _separator = " ";

        private bool _containsFreeText;

        /// <summary>
        /// Stores the lists of options
        /// </summary>
        private List<CheckBox> _options = new List<CheckBox>();

        private TextBox? _freeTextBox;

        public UIElement? CreateElement(IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            _name = fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            _separator = fieldData.getOrDefault<string>(_DatenMeister._Forms._CheckboxListTaggingFieldData.separator) ?? " ";
            _containsFreeText = fieldData.getOrDefault<bool>(_DatenMeister._Forms._CheckboxListTaggingFieldData.containsFreeText);

            var valuePairs =
                fieldData.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CheckboxListTaggingFieldData.values)?.ToList()
                ?? new List<object?>();
            
            var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly)
                             || fieldFlags.IsReadOnly;

            var currentValue = value.getOrDefault<string>(_name) ?? string.Empty;
            var copyCurrentValue = currentValue;
            var currentList = currentValue.Split(new [] {_separator}, StringSplitOptions.RemoveEmptyEntries);

            _options = new List<CheckBox>();
            foreach (var pair in valuePairs.OfType<IElement>())
            {
                var name = pair.getOrDefault<string>(_DatenMeister._Forms._ValuePair.name);
                var valueContent = pair.getOrDefault<string>(_DatenMeister._Forms._ValuePair.value);

                var checkbox = new CheckBox
                {
                    Content = name, 
                    Tag = valueContent,
                    IsEnabled = !isReadOnly, 
                    IsChecked = currentList.Contains(valueContent)
                };

                copyCurrentValue = copyCurrentValue.Replace(valueContent + _separator, string.Empty);
                copyCurrentValue = copyCurrentValue.Replace(valueContent, string.Empty);

                _options.Add(checkbox);
            }

            // Adds the stack panel
            var stackPanel = new StackPanel {Orientation = Orientation.Vertical};
            foreach (var option in _options)
            {
                stackPanel.Children.Add(option);
            }

            // Adds a freestyle text
            if (_containsFreeText)
            {
                _freeTextBox = new TextBox
                {
                    Text = copyCurrentValue,
                    IsReadOnly = isReadOnly
                };

                stackPanel.Children.Add(_freeTextBox);
            }

            return stackPanel;
        }


        /// <summary>
        /// This instance will be called, when the setting shall be performed upon the given element.
        /// This may be different as the one as specified in CreateElement
        /// </summary>
        /// <param name="element"> Element to be set</param>
        public void CallSetAction(IObject element)
        {
            var result = new StringBuilder();
            var separator = string.Empty;
            foreach (var option in _options)
            {
                if (option.IsChecked == true)
                {
                    result.Append(separator);
                    result.Append(option.Tag);
                    separator = _separator;
                }
            }

            // Free text
            var freeTextContent = (_containsFreeText && _freeTextBox != null) ? _freeTextBox.Text : string.Empty;
            if (freeTextContent != null && !string.IsNullOrEmpty(freeTextContent))
            {
                result.Append(separator);
                result.Append(freeTextContent);
            }

            element.set(_name, result.ToString());
        }
    }
}