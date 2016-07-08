using System;
using System.Globalization;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP.Forms
{
    public class DateTimeField : IField
    {
        public void CreateField(DetailFormHelper helper, FieldData fieldData)
        {
            var dateTimeFieldData = fieldData as DateTimeFieldData;
            if (dateTimeFieldData == null)
            {
                throw new ArgumentNullException(nameof(dateTimeFieldData));
            }

            int row;
            helper.AddRow(dateTimeFieldData, out row);
            
            // Gets the value
            var defaultTime = DateTime.Now;
            var valueObj =
                helper.DataElement.isSet(dateTimeFieldData.name) ?
                    helper.DataElement.get(dateTimeFieldData.name) :
                    (dateTimeFieldData.defaultValue as DateTime?) ?? DateTime.Now;

            DateTimeOffset valueDateTime;
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

            // Adds the fields, dependent on the configuration
            if (dateTimeFieldData.showDate)
            {
                dateTime.Date = asLocal;
                dateTime.CalendarIdentifier = CalendarIdentifiers.Gregorian;
                stackPanel.Children.Add(dateTime);
            }

            if (dateTimeFieldData.showTime)
            {
                time.Time = asLocal.TimeOfDay;
                time.ClockIdentifier = ClockIdentifiers.TwentyFourHour;
                stackPanel.Children.Add(time);
            }

            if (dateTimeFieldData.showOffsetButtons)
            {
                AddOffsetButton(stackPanel, dateTime, "+1 month", TimeSpan.FromDays(30));
                AddOffsetButton(stackPanel, dateTime, "+1 week", TimeSpan.FromDays(7));
                AddOffsetButton(stackPanel, dateTime, "+1 day", TimeSpan.FromDays(1));
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

        private static void AddOffsetButton(Panel stackPanel, DatePicker dateTime, string text, TimeSpan offset)
        {
            var button = new Button();
            button.Content = text;
            button.Click += (x, y) => { dateTime.Date = dateTime.Date.Add(offset); };
            stackPanel.Children.Add(button);
        }
    }
}