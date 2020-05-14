namespace TrainsOnline.Desktop.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public abstract class AbstractEnumToBooleanConverter<TEnum> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(parameter is string Parameter))
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(typeof(TEnum), value) == false)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(typeof(TEnum), Parameter).Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(parameter is string Parameter) ? DependencyProperty.UnsetValue : Enum.Parse(typeof(TEnum), Parameter);
        }
    }
}
