using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;
using Orientation = System.Windows.Controls.Orientation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class DateTimeField : IDetailField
    {
        private string? _name;

        private DateTime? _propertyValue;
        private TextBox? _datePanel;
        private TextBox? _timePanel;

        public UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            fieldFlags.CanBeFocused = true;
            
            _name = fieldData.get<string>(_DatenMeister._Forms._FieldData.name);
            var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._DateTimeFieldData.isReadOnly)
                             || fieldFlags.IsReadOnly;
            var hideDate = fieldData.getOrDefault<bool>(_DatenMeister._Forms._DateTimeFieldData.hideDate);
            var hideTime = fieldData.getOrDefault<bool>(_DatenMeister._Forms._DateTimeFieldData.hideTime);

            _propertyValue = DateTimeHelper.TruncateToSecond(DateTime.Now);
            
            if (value.isSet(_name))
            {
                _propertyValue = value.getOrDefault<DateTime>(_name);
            }
            else
            {
                if (fieldData.isSet(_DatenMeister._Forms._FieldData.defaultValue))
                {
                    _propertyValue = fieldData.getOrDefault<DateTime>(_DatenMeister._Forms._FieldData.defaultValue);
                }
            }

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            if (!hideDate)
            {
                _datePanel = new TextBox
                {
                    Text = _propertyValue.HasValue
                        ? _propertyValue.Value.Date.ToString("d", CultureInfo.CurrentCulture)
                        : string.Empty,
                    IsEnabled = !isReadOnly,
                    Width = 80,
                    HorizontalContentAlignment = HorizontalAlignment.Right
                };

                stackPanel.Children.Add(_datePanel);
            }
            
            if (!hideTime)
            {
                _timePanel = new TextBox
                {
                    Text = _propertyValue.HasValue
                        ? _propertyValue.Value.TimeOfDay.ToString("c", CultureInfo.CurrentCulture)
                        : string.Empty,
                    IsEnabled = !isReadOnly,
                    Width = 80,
                    HorizontalContentAlignment = HorizontalAlignment.Right
                };

                stackPanel.Children.Add(_timePanel);
            }

            return stackPanel;
        }

        public void CallSetAction(IObject element)
        {
            if (_name == null) return;
            var result = DateTime.MinValue;
            if (_datePanel != null)
            {
                if (DateTime.TryParseExact(
                    _datePanel.Text,
                    "d",
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out var dateTime))
                {
                    result = dateTime;
                }
            }

            if (_timePanel != null)
            {
                if (TimeSpan.TryParseExact(
                    _timePanel.Text,
                    "c",
                    CultureInfo.CurrentCulture,
                    out var span))
                {
                    result = result.Add(span);
                }
            }

            element.set(_name, result);
        }
    }
}