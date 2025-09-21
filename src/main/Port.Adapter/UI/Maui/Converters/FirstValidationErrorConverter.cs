using System.Globalization;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Converters;

public class FirstValidationErrorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<string> errors)
        {
            return errors?.FirstOrDefault() ?? string.Empty;
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
