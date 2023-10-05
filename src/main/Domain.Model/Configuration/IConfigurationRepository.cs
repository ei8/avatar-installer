namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Read avatar configuration values from the specified source
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<AvatarConfiguration> GetByAsync(string path);
    }
}
