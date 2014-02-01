using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace КВАНТ.CustomConverters
{
    public class ColorStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cl = (System.Windows.Media.Color)value;
            var st = cl.ToString();
            return st;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return new Color(); 
            var ret = (Color)ColorConverter.ConvertFromString((string)value);
            return ret;
            // return new BrushConverter().ConvertFromString((string)value) as SolidColorBrush;
        }
    }

    public class ColorStringConverterBrush : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cl = (SolidColorBrush)value;
            var st = cl.ToString();
            return st;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return new Color();
            var ret = (SolidColorBrush)new BrushConverter().ConvertFromString((string)value);
            return ret;
            // return new BrushConverter().ConvertFromString((string)value) as SolidColorBrush;
        }
    }

    public class TimeStringFormat : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cl = (DateTime)value;
            var st = cl.ToShortTimeString();
            return st;
        }
    }
}