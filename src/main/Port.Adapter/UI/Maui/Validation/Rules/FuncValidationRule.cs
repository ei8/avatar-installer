namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class FuncValidationRule<T> : IValidationRule<T>
{
    private readonly Func<T, bool> validate;

    public string ValidationMessage { get; set; }

    public FuncValidationRule(Func<T, bool> validate)
    {
        this.validate = validate;
    }

    public bool Check(T value) => this.validate(value);
}
