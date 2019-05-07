using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamShop.Converters
{
    public class SelectedToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((Boolean)value)
                    return Color.Orange;
                else
                    return Color.LightGray;
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Color.LightGray;
        }
    }
}
