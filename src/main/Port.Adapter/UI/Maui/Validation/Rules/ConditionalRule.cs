namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class ConditionalRule<T> : IValidationRule<T>
{
    private readonly Func<bool> conditionFunc;
    private readonly IValidationRule<T> innerRule;

    public string ValidationMessage { get; set; }

    public ConditionalRule(Func<bool> conditionFunc, IValidationRule<T> innerRule)
    {
        this.conditionFunc = conditionFunc;
        this.innerRule = innerRule;
        this.ValidationMessage = innerRule.ValidationMessage;
    }

    public bool Check(T value)
    {
        bool result = true;
        
        // If condition is met, apply the inner rule
        if (this.conditionFunc())
        {
            result = this.innerRule.Check(value);
        }
        // If condition is not met, validation passes (field is not required)
        
        return result;
    }
}
