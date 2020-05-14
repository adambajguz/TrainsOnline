namespace TrainsOnline.Desktop.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    internal class DateTimeFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
                return dateTime.ToLocalTime().ToString("dddd, dd-MM-yyyy hh:mm:ss");

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
