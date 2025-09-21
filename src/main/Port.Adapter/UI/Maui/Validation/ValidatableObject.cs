using CommunityToolkit.Mvvm.ComponentModel;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation;

public class ValidatableObject<T> : ObservableObject, IValidity
{
    private IEnumerable<string> errors;
    private bool isValid;
    private T value;

    public List<IValidationRule<T>> Validations { get; } = new();

    public IEnumerable<string> Errors
    {
        get => this.errors;
        private set => this.SetProperty(ref this.errors, value);
    }

    public bool IsValid
    {
        get => this.isValid;
        private set => this.SetProperty(ref this.isValid, value);
    }

    public T Value
    {
        get => this.value;
        set => this.SetProperty(ref this.value, value);
    }

    public ValidatableObject()
    {
        this.isValid = true;
        this.errors = Enumerable.Empty<string>();
    }

    public bool Validate()
    {
        this.Errors = Validations
            ?.Where(v => !v.Check(this.Value))
            ?.Select(v => v.ValidationMessage)
            ?.ToArray()
            ?? Enumerable.Empty<string>();

        this.IsValid = !this.Errors.Any();
        return this.IsValid;
    }
}
