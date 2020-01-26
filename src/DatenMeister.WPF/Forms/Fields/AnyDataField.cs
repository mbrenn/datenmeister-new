using System.Windows;
using System.Windows.Controls;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    public class AnyDataField : IDetailField
    {
        private RadioButton? _referenceRadioButton;
        private RadioButton? _textRadioButton;
        private TextBox? _textBoxForString;
        private ReferenceField? _referenceField;
        private string? _name;

        public UIElement? CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            var groupName = StringManipulation.RandomString(10);
            var stackPanel = new StackPanel {Orientation = Orientation.Vertical};
            var upperStackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            
            _name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            if (_name == null)
            {
                return new TextBlock
                {
                    Text = "Bad configuration: _name == null"
                };
            }

            var elementValue = value.getOrDefault<object>(_name);

            _textRadioButton = new RadioButton
            {
                GroupName = groupName,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            _textBoxForString = new TextBox
            {
                Text = "",
                Width = 300,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            _textRadioButton.Content = _textBoxForString;

            upperStackPanel.Children.Add(_textRadioButton);

            var lowerStackPanel = new StackPanel {Orientation = Orientation.Horizontal};

            _referenceRadioButton = new RadioButton
            {
                GroupName = groupName
            };
            
            _referenceField = new ReferenceField();
            var panel = _referenceField.CreateElement(value, fieldData, detailForm, fieldFlags);
            _referenceRadioButton.Content = panel;

            lowerStackPanel.Children.Add(_referenceRadioButton);

            stackPanel.Children.Add(upperStackPanel);
            stackPanel.Children.Add(lowerStackPanel);
            stackPanel.Margin = new Thickness(10);

            if (elementValue != null && DotNetHelper.IsOfPrimitiveType(elementValue))
            {
                _textRadioButton.IsChecked = true;
                _textBoxForString.Text = elementValue.ToString();
            }
            else
            {
                _referenceRadioButton.IsChecked = true;
                _referenceField.SetSelectedValue(elementValue);
                _textBoxForString.IsEnabled = false;
            }
            
            _referenceRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = false;
                _referenceField.IsEnabled = true;
            };

            _textRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = true;
                _referenceField.IsEnabled = false;
            };
            
            return stackPanel;
        }

        public void CallSetAction(IObject element)
        {
            if (_name == null) return;
            if (_textBoxForString == null) return;
            if (_referenceField == null) return;
            
            if (_textRadioButton?.IsChecked == true)
            {
                element.set(_name, _textBoxForString.Text);
            }

            if (_referenceRadioButton?.IsChecked == true)
            {
                if (_referenceField.IsValueUnsetted)
                {
                    element.unset(_name);
                }
                else
                {
                    element.set(_name, _referenceField.SelectedValue);                    
                }
            }
        }
    }
}