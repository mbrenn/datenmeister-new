using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP.Forms
{
    public class TextField : IField
    {
        public void CreateField(DetailFormHelper helper, FieldData fieldData)
        {
            var text = fieldData as TextFieldData;
            int row;
            var textBlock  = helper.AddRow(text, out row);

            var textBox = new TextBox
            {
                Text = DetailFormHelper.GetValueAsString(helper.DataElement, text.name, fieldData.defaultValue),
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            };

            var lineHeight = Math.Max(1, text.lineHeight);
            textBox.Height = 20 * lineHeight;
            if (lineHeight > 1)
            {
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBox.AcceptsReturn = true;
            }

            Grid.SetColumn(textBox, 1);
            Grid.SetRow(textBox, row);

            helper.GridFields.Children.Add(textBox);

            helper.BindingActions.Add(
                () => helper.DataElement.set(text.name, textBox.Text));
        }
    }
}