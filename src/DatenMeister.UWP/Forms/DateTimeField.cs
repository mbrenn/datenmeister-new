using System;
using System.Globalization;
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
            
            // Gets the value
            var defaultTime = DateTime.Now;
            var valueObj =
                helper.DataElement.isSet(dateTimeFieldData.name) ?
                    helper.DataElement.get(dateTimeFieldData.name) :
                    DateTime.Now;

            DateTimeOffset valueDateTime = DateTimeOffset.Now.ToUniversalTime();
            if (!(valueObj is DateTime))
            {
                if (!DateTimeOffset.TryParse(valueObj.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out valueDateTime))
                {
                    valueDateTime = defaultTime;
                }
            }
            else
            {
                valueDateTime = defaultTime;
            }

            var asLocal = valueDateTime.ToLocalTime();


            // Adds the fields, dependent on the configuration
            if (dateTimeFieldData.showDate)
            {
                dateTime.Date = asLocal;
                stackPanel.Children.Add(dateTime);
            }

            if (dateTimeFieldData.showTime)
            {
                time.Time = asLocal.TimeOfDay;
                stackPanel.Children.Add(time);
            }

            helper.GridFields.Children.Add(stackPanel);

            // Sets the values
            helper.BindingActions.Add(
                () =>
                {
                    var value = new DateTime(2000,1,1);

                    if (dateTimeFieldData.showDate)
                    {
                        value = dateTime.Date.Date;
                    }

                    if (dateTimeFieldData.showTime)
                    {
                        value = value.Add(time.Time);
                    }

                    helper.DataElement.set(dateTimeFieldData.name, value);
                });
        }
    }
}