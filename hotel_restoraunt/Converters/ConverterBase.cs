using System;
using System.Globalization;
using System.Windows.Data;

namespace hotel_restoraunt.Converters
{
    public abstract class ConverterBase : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace hotel_restoraunt.Converters
{
    public abstract class ConverterBase : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}