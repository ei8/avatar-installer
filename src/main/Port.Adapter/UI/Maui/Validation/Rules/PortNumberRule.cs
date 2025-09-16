namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class PortNumberRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }

    public bool Check(T value)
    {
        if (value is not string port || string.IsNullOrWhiteSpace(port))
            return false;

        if (!int.TryParse(port, out int portNumber))
            return false;

        return portNumber >= 1 && portNumber <= 65535;
    }
}
