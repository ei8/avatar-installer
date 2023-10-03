namespace ei8.Avatar.Installer.Domain.Model.Template
{
    public interface ITemplateService
    {
        Task RetrieveTemplateAsync(string destinationPath);

        void EnumerateTemplateFiles(string templatePath);
    }
}