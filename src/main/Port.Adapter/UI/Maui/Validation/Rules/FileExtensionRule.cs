namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;

public class FileExtensionRule<T> : IValidationRule<T>
{
    private readonly string[] allowedExtensions;
    
    public string ValidationMessage { get; set; }

    public FileExtensionRule(params string[] allowedExtensions)
    {
        this.allowedExtensions = allowedExtensions.Select(ext => ext.ToLower()).ToArray();
    }

    public bool Check(T value)
    {
        bool result = false;
        
        if (value is string filePath && !string.IsNullOrWhiteSpace(filePath))
        {
            var extension = Path.GetExtension(filePath)?.ToLower();
            result = !string.IsNullOrEmpty(extension) && this.allowedExtensions.Contains(extension);
        }
        
        return result;
    }
}
