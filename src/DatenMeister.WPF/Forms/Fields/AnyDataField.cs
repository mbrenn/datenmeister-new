using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class AnyDataField : IDetailField
    {
        private RadioButton? _referenceRadioButton;
        private RadioButton? _textRadioButton;
        private TextBox? _textBoxForString;
        private ReferenceField? _referenceField;
        private string? _name;
        private IObject? _element;
        private DetailFormControl? _detailFormControl;

        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            _element = value;
            _detailFormControl = detailForm;
            
            var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._AnyDataFieldData.isReadOnly) ||
                             fieldFlags.IsReadOnly;

            _name = fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
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

            // When element is known to be an element, try to read it as an element
            // if it does not return a value, then try to read it in as an object
            var elementValue = isPrimitive
                ? value.getOrDefault<object>(_name)
                : value.getOrDefault<IElement>(_name) ?? value.getOrDefault<object>(_name);

            if (isReadOnly)
            {
                var textBlock = new TextBlock();
                if (elementValue != null && DotNetHelper.IsOfPrimitiveType(elementValue))
                {
                    textBlock.Text = elementValue.ToString();
                }
                else if (elementValue is IObject asObject)
                {
                    ReferenceField.UpdateTextOfTextBlock(textBlock, asObject);
                    textBlock.MouseDown += TextBlockOnMouseDown;
                }
                else
                {
                    textBlock.Text = "Unknown";
                }

                return textBlock;
            }

            var groupName = StringManipulation.RandomString(10);
            var stackPanel = new StackPanel
                {Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Stretch};


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
            _referenceRadioButton.IsEnabled = !isReadOnly;
            _referenceRadioButton.Content = panel;


            stackPanel.Children.Add(_textRadioButton);
            stackPanel.Children.Add(_referenceRadioButton);
            stackPanel.Margin = new Thickness(10);

            _textRadioButton.IsEnabled = !isReadOnly;

            if (elementValue != null && DotNetHelper.IsOfPrimitiveType(elementValue))
            {
                _textRadioButton.IsChecked = true;
                _textBoxForString.Text = elementValue.ToString();
                _textBoxForString.IsEnabled = !isReadOnly;
                _referenceField.IsEnabled = false;
            }
            else
            {
                _referenceRadioButton.IsChecked = true;
                _referenceField.SetSelectedValue(elementValue);
                _textBoxForString.IsEnabled = false;
                _textBoxForString.IsEnabled = false;
                _referenceField.IsEnabled = true;
            }

            _referenceRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = false;
                _referenceField.IsEnabled = true;
            };

            _textRadioButton.Checked += (x, y) =>
            {
                _textBoxForString.IsEnabled = !isReadOnly;
                _referenceField.IsEnabled = false;
            };

            return stackPanel;
        }

        private void TextBlockOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_name == null) throw new InvalidOperationException("_name == null");
            if (_element == null) throw new InvalidOperationException("_value == null");
            if (_detailFormControl == null) throw new InvalidOperationException("_detailFormControl == null");

            var itemToOpen = _element.getOrDefault<IElement>(_name);
            if (itemToOpen == null)
            {
                MessageBox.Show("No item selected");
            }
            else
            {
                _ = NavigatorForItems.NavigateToElementDetailView(
                    _detailFormControl.NavigationHost,
                    itemToOpen);
            }
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