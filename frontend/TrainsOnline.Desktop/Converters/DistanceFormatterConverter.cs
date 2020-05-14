namespace TrainsOnline.Desktop.Converters
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml.Data;

    internal class DistanceFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return string.Empty;

            string text = value.ToString();

            if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double dbl))
            {
                text = dbl.ToString("0.0 km", CultureInfo.InvariantCulture);
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
