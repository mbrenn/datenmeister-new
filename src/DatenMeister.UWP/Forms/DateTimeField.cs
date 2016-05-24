using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP.Forms
{
    public class DateTimeField : IField
    {
        public void CreateField(DetailFormHelper helper, FieldData field)
        {
            var dateTimeFieldData = field as DateTimeFieldData;
            if (dateTimeFieldData == null)
            {
                throw new ArgumentNullException(nameof(dateTimeFieldData));
            }

            int row;
            helper.AddRow(dateTimeFieldData, out row);

            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            var dateTime = new DatePicker
            {
                Margin = new Thickness(5)
            };

            var time = new TimePicker
            {
                Margin = new Thickness(5)
            };

            Grid.SetColumn(stackPanel, 1);
            Grid.SetRow(stackPanel, row);

            if (dateTimeFieldData.showDate)
            {
                stackPanel.Children.Add(dateTime);
            }

            if (dateTimeFieldData.showTime)
            {
                stackPanel.Children.Add(time);
            }

            helper.GridFields.Children.Add(stackPanel);

            helper.BindingActions.Add(
                () => helper.DataElement.set(dateTimeFieldData.name, dateTime.Date.Date));
        }
    }
}