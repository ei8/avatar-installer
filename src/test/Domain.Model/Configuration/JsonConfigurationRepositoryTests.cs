using ei8.Avatar.Installer.Domain.Model.Configuration;

namespace Domain.Model.Test.Configuration
{
    public class JsonConfigurationRepositoryTests
    {
        [Fact]
        public async Task LoadsAllJsonPropertiesIntoAvatarConfiguration()
        {
            var jsonFile = "./Configuration/testcase_single.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByAsync(jsonFile);

            Assert.Equal("./sample", result.Destination);

            Assert.Single(result.Avatars);
            Assert.Equal("junvic", result.Avatars[0].Name);

            // cortex_graph
            Assert.Equal("graph", result.Avatars[0].CortexGraph!.DbName);
            Assert.Equal("root", result.Avatars[0].CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", result.Avatars[0].CortexGraph!.DbUrl);
            Assert.Equal("", result.Avatars[0].CortexGraph!.ArangoRootPassword);

            // avatar_api
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].AvatarApi!.TokenIssuerUrl);
            Assert.Equal("avatarapi-junvic", result.Avatars[0].AvatarApi!.ApiName);

            // cortex_library
            Assert.Equal("http://fibona.cc/junvic/cortex/neurons", result.Avatars[0].CortexLibrary!.NeuronsUrl);
            Assert.Equal("http://fibona.cc/junvic/cortex/terminals", result.Avatars[0].CortexLibrary!.TerminalsUrl);

            // d23
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].D23!.OidcAuthorityUrl);
            Assert.Equal("d23-junvic", result.Avatars[0].D23!.ClientId);
            Assert.Equal("/junvic/d23", result.Avatars[0].D23!.BasePath);

            // network
            Assert.Equal("192.168.1.110", result.Avatars[0].Network!.LocalIp);
            Assert.Equal(64101, result.Avatars[0].Network!.AvatarInPort);
            Assert.Equal(64103, result.Avatars[0].Network!.D23BlazorPort);
            Assert.Equal(60, result.Avatars[0].Network!.Ssh!.ServerAliveInterval);
            Assert.Equal(525600, result.Avatars[0].Network!.Ssh!.ServerAliveCountMax);
            Assert.Equal(2222, result.Avatars[0].Network!.Ssh!.Port);
            Assert.Equal("ei8.host", result.Avatars[0].Network!.Ssh!.HostName);
            Assert.Equal("jv:80", result.Avatars[0].Network!.Ssh!.RemoteForward);
            Assert.Equal(9393, result.Avatars[0].Network!.Ssh!.LocalPort);
        }

        [Fact]
        public async Task UsesDefaults_ForUndefinedConfigurationProperties()
        {
            var jsonFile = "./Configuration/testcase_optionalfields.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByAsync(jsonFile);

            Assert.Single(result.Avatars);
            Assert.Equal("valdez", result.Avatars[0].Name);

            // cortex_graph
            Assert.Equal("graph", result.Avatars[0].CortexGraph!.DbName);
            Assert.Equal("root", result.Avatars[0].CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", result.Avatars[0].CortexGraph!.DbUrl);
            Assert.Equal("", result.Avatars[0].CortexGraph!.ArangoRootPassword);

            // avatar_api - should infer defaults
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].AvatarApi!.TokenIssuerUrl);
            Assert.Equal("avatarapi-valdez", result.Avatars[0].AvatarApi!.ApiName);

            // cortex_library - should infer defaults
            Assert.Equal("http://fibona.cc/valdez/cortex/neurons", result.Avatars[0].CortexLibrary!.NeuronsUrl);
            Assert.Equal("http://fibona.cc/valdez/cortex/terminals", result.Avatars[0].CortexLibrary!.TerminalsUrl);

            // d23 - should infer defaults
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].D23!.OidcAuthorityUrl);
            Assert.Equal("d23-valdez", result.Avatars[0].D23!.ClientId);
            Assert.Equal("/valdez/d23", result.Avatars[0].D23!.BasePath);

            // network - should infer defaults
            Assert.Equal("192.168.50.2", result.Avatars[0].Network!.LocalIp);
            Assert.Equal(12345, result.Avatars[0].Network!.AvatarInPort);
            Assert.Equal(64103, result.Avatars[0].Network!.D23BlazorPort);
            Assert.Equal(90, result.Avatars[0].Network!.Ssh!.ServerAliveInterval);
            Assert.Equal(365000, result.Avatars[0].Network!.Ssh!.ServerAliveCountMax);
            Assert.Equal(2222, result.Avatars[0].Network!.Ssh!.Port);
            Assert.Equal("ei8.host", result.Avatars[0].Network!.Ssh!.HostName);
            Assert.Equal("jv:8080", result.Avatars[0].Network!.Ssh!.RemoteForward);
            Assert.Equal(9393, result.Avatars[0].Network!.Ssh!.LocalPort);
        }

        [Fact]
        public async Task LoadsAllAvatarConfigurations()
        {
            var jsonFile = "./Configuration/testcase_multiple.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByAsync(jsonFile);

            Assert.Equal(2, result.Avatars.Count());

            Assert.Collection(result.Avatars,
                (a) => AssertDefaultValues("avatar-work", a),
                (b) => AssertDefaultValues("avatar-personal", b));
        }

        private void AssertDefaultValues(string avatarName, AvatarConfigurationItem avatar)
        {
            Assert.Equal(avatarName, avatar.Name);

            // cortex_graph
            Assert.Equal("graph", avatar.CortexGraph!.DbName);
            Assert.Equal("root", avatar.CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", avatar.CortexGraph!.DbUrl);
            Assert.Equal("", avatar.CortexGraph!.ArangoRootPassword);

            // avatar_api
            Assert.Equal("https://login.fibona.cc", avatar.AvatarApi!.TokenIssuerUrl);
            Assert.Equal($"avatarapi-{avatarName}", avatar.AvatarApi!.ApiName);

            // cortex_library
            Assert.Equal($"http://fibona.cc/{avatarName}/cortex/neurons", avatar.CortexLibrary!.NeuronsUrl);
            Assert.Equal($"http://fibona.cc/{avatarName}/cortex/terminals", avatar.CortexLibrary!.TerminalsUrl);

            // d23
            Assert.Equal("https://login.fibona.cc", avatar.D23!.OidcAuthorityUrl);
            Assert.Equal($"d23-{avatarName}", avatar.D23!.ClientId);
            Assert.Equal($"/{avatarName}/d23", avatar.D23!.BasePath);

            // network
            Assert.Equal("192.168.1.110", avatar.Network!.LocalIp);
            Assert.Equal(64101, avatar.Network!.AvatarInPort);
            Assert.Equal(64103, avatar.Network!.D23BlazorPort);
            Assert.Equal(60, avatar.Network!.Ssh!.ServerAliveInterval);
            Assert.Equal(525600, avatar.Network!.Ssh!.ServerAliveCountMax);
            Assert.Equal(2222, avatar.Network!.Ssh!.Port);
            Assert.Equal("ei8.host", avatar.Network!.Ssh!.HostName);
            Assert.Equal($"{avatarName}:80", avatar.Network!.Ssh!.RemoteForward);
            Assert.Equal(9393, avatar.Network!.Ssh!.LocalPort);
        }
    }
}