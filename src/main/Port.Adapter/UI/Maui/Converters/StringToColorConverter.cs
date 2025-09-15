using System.Globalization;
using Microsoft.Maui.Graphics;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Converters;

public class StringToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && parameter is string colorString)
        {
            var colors = colorString.Split('|');
            if (colors.Length == 2)
            {
                return string.IsNullOrEmpty(stringValue) ? colors[1] : colors[0];
            }
        }
        return Colors.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
