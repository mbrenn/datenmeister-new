using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DatenMeister.UWP.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(formatString, value);
            }

            if (value is DateTime)
            {
                return ((DateTime)value).ToString(CultureInfo.CurrentUICulture);
            }

            return value.ToString();
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
