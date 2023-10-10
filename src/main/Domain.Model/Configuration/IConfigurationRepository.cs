namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Read avatar configuration values from the specified source
        /// </summary>
        /// <param name="id">The identifier for the configuration source</param>
        /// <returns></returns>
        Task<AvatarConfiguration> GetByAsync(string id);
    }
}
