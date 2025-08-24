namespace ei8.Avatar.Installer.Domain.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnvironmentVariableKeyAttribute : Attribute
    {
        public EnvironmentVariableKeyAttribute(string key)
        {
            this.Key = key;
        }

        public string Key { get; }
    }
}
