namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class ConditionalRule<T> : IValidationRule<T>
{
    private readonly Func<bool> _conditionFunc;
    private readonly IValidationRule<T> _innerRule;

    public string ValidationMessage { get; set; }

    public ConditionalRule(Func<bool> conditionFunc, IValidationRule<T> innerRule)
    {
        _conditionFunc = conditionFunc;
        _innerRule = innerRule;
        ValidationMessage = innerRule.ValidationMessage;
    }

    public bool Check(T value)
    {
        // If condition is not met, validation passes (field is not required)
        if (!_conditionFunc())
            return true;

        // If condition is met, apply the inner rule
        return _innerRule.Check(value);
    }
}
