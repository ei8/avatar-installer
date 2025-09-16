namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }

    public bool Check(T value) =>
        value is string str && !string.IsNullOrWhiteSpace(str);
}
