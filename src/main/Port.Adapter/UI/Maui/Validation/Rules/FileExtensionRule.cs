namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class FileExtensionRule<T> : IValidationRule<T>
{
    private readonly string[] _allowedExtensions;
    
    public string ValidationMessage { get; set; }

    public FileExtensionRule(params string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions.Select(ext => ext.ToLower()).ToArray();
    }

    public bool Check(T value)
    {
        if (value is not string filePath || string.IsNullOrWhiteSpace(filePath))
            return false;

        var extension = Path.GetExtension(filePath)?.ToLower();
        return !string.IsNullOrEmpty(extension) && _allowedExtensions.Contains(extension);
    }
}
