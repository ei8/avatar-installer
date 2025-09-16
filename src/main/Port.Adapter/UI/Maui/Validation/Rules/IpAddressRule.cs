using System.Net;
using System.Text.RegularExpressions;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class IpAddressRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }

    public bool Check(T value)
    {
        if (value is not string ipAddress || string.IsNullOrWhiteSpace(ipAddress))
            return false;

        // Use regex to validate IP format (0-255.0-255.0-255.0-255)
        var ipPattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        
        if (!Regex.IsMatch(ipAddress, ipPattern))
            return false;

        // Additional validation using IPAddress.TryParse
        return IPAddress.TryParse(ipAddress, out _);
    }
}
