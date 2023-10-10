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
            public override string ConvertName(string name) => ToSnakeCase(name);

            private string ToSnakeCase(string name)
            {
                // insert an underscore before each char that is uppercase
                return string.Concat(name.Select((x, i) =>
                {
                    if (i > 0 && char.IsUpper(x))
                        return "_" + x.ToString();
                    else
                        return x.ToString();
                })).ToLower();
            }
        }
    }
}
