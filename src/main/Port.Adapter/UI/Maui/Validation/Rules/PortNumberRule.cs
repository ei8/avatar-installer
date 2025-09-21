namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class PortNumberRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }

    public bool Check(T value)
    {
        bool result = false;
        
        if (value is string port && !string.IsNullOrWhiteSpace(port))
        {
            if (int.TryParse(port, out int portNumber))
            {
                result = portNumber >= 1 && portNumber <= 65535;
            }
        }
        
        return result;
    }
}
