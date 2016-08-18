using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.Models.Forms;

namespace DatenMeister.UWP.Forms
{
    public class DropDownField : IField
    {
        public void CreateField(DetailFormHelper helper, FieldData fieldData)
        {
            var dropDownField = fieldData as DropDownFieldData;
            if (dropDownField == null)
            {
                throw new ArgumentNullException(nameof(dropDownField));
            }


            int rowNr;
            helper.AddRow(fieldData, out rowNr);

            var itemsSource = dropDownField.values.Select(x => new DropDownItem(x)).ToList();
            var dropDown = new ComboBox
            {
                ItemsSource = itemsSource,
                Margin = new Thickness(5),
                Width = 200
            };

            // Tries to find the selected element
            var value = DetailFormHelper.GetValue(helper.DataElement, fieldData.name, fieldData.defaultValue);
            var found = itemsSource.FirstOrDefault(x => x.value?.Equals(value) == true);
            dropDown.SelectedItem = found;

            Grid.SetColumn(dropDown, 1);
            Grid.SetRow(dropDown, rowNr);

            helper.GridFields.Children.Add(dropDown);

            helper.BindingActions.Add(
                () =>
                {
                    var selectedValue = dropDown.SelectedValue as DropDownItem;
                    if (selectedValue != null)
                    {
                        helper.DataElement.set(fieldData.name, selectedValue?.value);
                    }
                    else
                    {
                        helper.DataElement.unset(fieldData.name);
                    }
                });
        }

        private class DropDownItem : DropDownFieldData.ValuePair
        {
            public DropDownItem(DropDownFieldData.ValuePair x) : base(x.value, x.name)
            {
            }

            public DropDownItem(string value, string name) : base(value, name)
            {
            }

            public override string ToString()
            {
                return name;
            }
        }
    }
}