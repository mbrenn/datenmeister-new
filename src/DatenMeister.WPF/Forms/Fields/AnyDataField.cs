using System.Windows;
using System.Windows.Controls;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
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
            var isReadOnly = fieldData.getOrDefault<bool>(_FormAndFields._AnyDataFieldData.isReadOnly) ||
                             fieldFlags.IsReadOnly;
            
            var groupName = StringManipulation.RandomString(10);
            var stackPanel = new StackPanel
                {Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Stretch};
            
            _name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            if (_name == null)
            {
                return new TextBlock
                {
                    Text = "Bad configuration: _name == null"
                };
            }

            var isPrimitive = true;
            var metaClass = (value as IElement)?.getMetaClass();
            if (metaClass != null)
            {
                var propertyType = ClassifierMethods.GetPropertyOfClassifier(metaClass, _name);
                if (propertyType != null)
                {
                    isPrimitive = ClassifierMethods.IsOfPrimitiveType(propertyType);
                }
            }

            var elementValue = isPrimitive 
                ? value.getOrDefault<object>(_name)
                : value.getOrDefault<IElement>(_name);

            _textRadioButton = new RadioButton
            {
                GroupName = groupName,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            _textBoxForString = new TextBox
            {
                Text = "",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            
            _textRadioButton.Content = _textBoxForString;

            _referenceRadioButton = new RadioButton
            {
                GroupName = groupName,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            
            _referenceField = new ReferenceField();
            var panel = _referenceField.CreateElement(value, fieldData, detailForm, fieldFlags);
            _referenceRadioButton.Content = panel;

            stackPanel.Children.Add(_textRadioButton);
            stackPanel.Children.Add(_referenceRadioButton);
            stackPanel.Margin = new Thickness(10);

            if (elementValue != null && DotNetHelper.IsOfPrimitiveType(elementValue))
            {
                _textRadioButton.IsChecked = true;
                _textRadioButton.IsEnabled = !isReadOnly;
                _textBoxForString.Text = elementValue.ToString();
            }
            else
            {
                _referenceRadioButton.IsChecked = true;
                _referenceRadioButton.IsEnabled = !isReadOnly;
                _referenceField.SetSelectedValue(elementValue);
                _textBoxForString.IsEnabled = false;
            }
            
            _referenceRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = false;
                _referenceField.IsEnabled = !isReadOnly;
            };

            _textRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = !isReadOnly;
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