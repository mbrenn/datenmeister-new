using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    /// <summary>
    /// Method being used for a dropdown field
    /// </summary>
    public class DropdownField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            ref FieldFlags fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            var name = fieldData.get(_FormAndFields._FieldData.name).ToString();
            object propertyValue = null;
            if (value.isSet(name))
            {
                propertyValue = value.get(name);
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    propertyValue = fieldData.get(_FormAndFields._FieldData.defaultValue);
                }
            }

            var dropDownValues = fieldData.getOrDefault(_FormAndFields._DropDownFieldData.values);
            if (dropDownValues == null)
            {
                return new TextBlock
                {
                    Text = "No values are given"
                };
            }

            var combobox = new ComboBox();
            var items = new List<ComboBoxItem>();
            ComboBoxItem selectedBoxItem = null;

            foreach (var itemPair in DotNetHelper.AsEnumeration(dropDownValues).Select(x=> x as IElement))
            {
                var nameOfItem = itemPair.getOrDefault(_FormAndFields._ValuePair.name);
                var valueOfItem = itemPair.getOrDefault(_FormAndFields._ValuePair.value);

                if (nameOfItem != null && valueOfItem != null)
                {
                    var comboBoxItem = new ComboBoxItem
                    {
                        Content = nameOfItem.ToString(),
                        Tag = valueOfItem
                    };

                    items.Add(comboBoxItem);

                    if (valueOfItem.Equals(propertyValue))
                    {
                        selectedBoxItem = comboBoxItem;
                    }
                }
            }

            combobox.ItemsSource = items;
            combobox.SelectedValue = selectedBoxItem;

            detailForm.SetActions.Add(
                element =>
                {
                    if (propertyValue != combobox.SelectedValue)
                    {
                        element.set(name, (combobox.SelectedValue as ComboBoxItem)?.Tag);
                    }
                });

            return combobox;
        }
    }
}