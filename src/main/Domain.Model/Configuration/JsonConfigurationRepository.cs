using ei8.Avatar.Installer.Common;
using System.Text.Json;

namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    public class JsonConfigurationRepository : IConfigurationRepository
    {
        public async Task<AvatarConfiguration> GetByAsync(string id)
        {
            if (!File.Exists(id))
                throw new FileNotFoundException($"{id} does not exist.");

            using (var file = File.OpenRead(id))
            {
                return await JsonSerializer.DeserializeAsync<AvatarConfiguration>(file, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                });
            }
        }

        private class SnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.ToSnakeCase();
        }
    }
}
