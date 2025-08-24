using ei8.Avatar.Installer.Domain.Model.Configuration;

namespace Domain.Model.Test.Configuration
{
    public class JsonConfigurationRepositoryFixture
    {
        [Fact]
        public async Task LoadsAllJsonPropertiesIntoAvatarConfiguration()
        {
            var jsonFile = "./Configuration/testcase_single.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByIdAsync(jsonFile);

            Assert.Equal("./sample", result.Destination);

            Assert.Single(result.Avatars);
            Assert.Equal("junvic", result.Avatars[0].Name);

            Assert.Equal("neurul.net", result.Avatars[0].Routing!.neurULServerDomainName);

            // cortex_graph
            Assert.Equal("graph", result.Avatars[0].CortexGraph!.DbName);
            Assert.Equal("root", result.Avatars[0].CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", result.Avatars[0].CortexGraph!.DbUrl);
            Assert.Equal("", result.Avatars[0].CortexGraphPersistence!.ArangoRootPassword);

            // avatar_api
            Assert.Equal("https://www.example.com/token", result.Avatars[0].AvatarApi!.TokenIssuerAddress);
            Assert.Equal("avatarapi-junvic", result.Avatars[0].AvatarApi!.ApiName);

            // cortex_library
            Assert.Equal("http://fibona.cc/junvic/cortex/neurons", result.Avatars[0].CortexLibrary!.NeuronsUrl);
            Assert.Equal("http://fibona.cc/junvic/cortex/terminals", result.Avatars[0].CortexLibrary!.TerminalsUrl);

            // un8y
            Assert.Equal("https://www.example.com/oidc", result.Avatars[0].Un8y!.OidcAuthorityUrl);
            Assert.Equal("un8y-junvic", result.Avatars[0].Un8y!.ClientId);
            Assert.Equal("/junvic/un8y", result.Avatars[0].Un8y!.BasePath);
            Assert.Equal("mypass", result.Avatars[0].Un8y!.CertificatePassword);
            Assert.Equal("/mypath/my.pfx", result.Avatars[0].Un8y!.CertificatePath);

            // orchestration
            Assert.Equal("192.168.1.110", result.Avatars[0].Orchestration!.AvatarIp);
            Assert.Equal("192.168.1.111", result.Avatars[0].Orchestration!.Un8yIp);
            Assert.Equal(64101, result.Avatars[0].Orchestration!.AvatarInPort);
            Assert.Equal(64103, result.Avatars[0].Orchestration!.Un8yBlazorPort);
            Assert.Equal("my/keys", result.Avatars[0].Orchestration!.KeysPath);

            // event sourcing
            Assert.Equal("/key/path", result.Avatars[0].EventSourcing!.PrivateKeyPath);
            Assert.Equal("/key/inprocesspath", result.Avatars[0].EventSourcing!.InProcessPrivateKeyPath);
            Assert.True(result.Avatars[0].EventSourcing!.EncryptionEnabled);
            Assert.Equal("encryptedkey", result.Avatars[0].EventSourcing!.EncryptedEventsKey);

            // root network
            Assert.Equal(60, result.Network!.Ssh!.ServerAliveInterval);
            Assert.Equal(525600, result.Network!.Ssh!.ServerAliveCountMax);
            Assert.Equal(2222, result.Network!.Ssh!.Port);
            Assert.Equal("ei8.host", result.Network!.Ssh!.HostName);
            Assert.Equal("jv:80", result.Network!.Ssh!.RemoteForward);
            Assert.Equal(9393, result.Network!.Ssh!.LocalPort);
        }

        [Fact]
        public async Task UsesDefaults_ForUndefinedConfigurationProperties()
        {
            var jsonFile = "./Configuration/testcase_optionalfields.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByIdAsync(jsonFile);

            Assert.Single(result.Avatars);
            Assert.Equal("valdez", result.Avatars[0].Name);

            Assert.Equal("fibona.cc", result.Avatars[0].Routing!.neurULServerDomainName);

            // cortex_graph
            Assert.Equal("graph", result.Avatars[0].CortexGraph!.DbName);
            Assert.Equal("root", result.Avatars[0].CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", result.Avatars[0].CortexGraph!.DbUrl);
            Assert.Equal("", result.Avatars[0].CortexGraphPersistence!.ArangoRootPassword);

            // avatar_api - should infer defaults
            Assert.Equal("Guest", result.Avatars[0].AvatarApi!.AnonymousUserId);
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].AvatarApi!.TokenIssuerAddress);
            Assert.Equal("avatarapi-valdez", result.Avatars[0].AvatarApi!.ApiName);

            // cortex_library - should infer defaults
            Assert.Equal("https://fibona.cc/valdez/cortex/neurons", result.Avatars[0].CortexLibrary!.NeuronsUrl);
            Assert.Equal("https://fibona.cc/valdez/cortex/terminals", result.Avatars[0].CortexLibrary!.TerminalsUrl);

            // un8y - should infer defaults
            Assert.Equal("https://login.fibona.cc", result.Avatars[0].Un8y!.OidcAuthorityUrl);
            Assert.Equal("un8y-valdez", result.Avatars[0].Un8y!.ClientId);
            Assert.Equal("/valdez/un8y", result.Avatars[0].Un8y!.BasePath);
            Assert.Equal(string.Empty, result.Avatars[0].Un8y!.CertificatePassword);
            Assert.Equal("/https/aspnetapp.pfx", result.Avatars[0].Un8y!.CertificatePath);

            // orchestration - should infer defaults
            Assert.Equal("192.168.50.2", result.Avatars[0].Orchestration!.AvatarIp);
            Assert.Equal("192.168.50.3", result.Avatars[0].Orchestration!.Un8yIp);
            Assert.Equal(12345, result.Avatars[0].Orchestration!.AvatarInPort);
            Assert.Equal(23456, result.Avatars[0].Orchestration!.Un8yBlazorPort);
            Assert.Equal(string.Empty, result.Avatars[0].Orchestration!.KeysPath);

            // event sourcing
            Assert.Equal("/C/keys/private.key", result.Avatars[0].EventSourcing!.PrivateKeyPath);
            Assert.Equal(string.Empty, result.Avatars[0].EventSourcing!.InProcessPrivateKeyPath);
            Assert.False(result.Avatars[0].EventSourcing!.EncryptionEnabled);
            Assert.Equal(string.Empty, result.Avatars[0].EventSourcing!.EncryptedEventsKey);

            // root network - should infer defaults
            Assert.Equal(90, result.Network!.Ssh!.ServerAliveInterval);
            Assert.Equal(365000, result.Network!.Ssh!.ServerAliveCountMax);
            Assert.Equal(2222, result.Network!.Ssh!.Port);
            Assert.Equal("ei8.host", result.Network!.Ssh!.HostName);
            Assert.Equal("jv:8080", result.Network!.Ssh!.RemoteForward);
            Assert.Equal(9393, result.Network!.Ssh!.LocalPort);
        }

        [Fact]
        public async Task LoadsAllAvatarConfigurations()
        {
            var jsonFile = "./Configuration/testcase_multiple.json";

            var sut = new JsonConfigurationRepository();
            var result = await sut.GetByIdAsync(jsonFile);

            Assert.Equal(2, result.Avatars.Count());

            Assert.Collection(result.Avatars,
                (a) => AssertDefaultValues("avatar-work", a),
                (b) => AssertDefaultValues("avatar-personal", b));
        }

        private void AssertDefaultValues(string avatarName, AvatarConfigurationItem avatar)
        {
            Assert.Equal(avatarName, avatar.Name);

            // routing
            Assert.Equal("fibona.cc", avatar.Routing!.neurULServerDomainName);

            // cortex_graph
            Assert.Equal("graph_" + avatarName, avatar.CortexGraph!.DbName);
            Assert.Equal("root", avatar.CortexGraph!.DbUsername);
            Assert.Equal("http://cortex.graph.persistence:8529", avatar.CortexGraph!.DbUrl);
            Assert.Equal("", avatar.CortexGraphPersistence!.ArangoRootPassword);

            // avatar_api
            Assert.Equal("https://login.fibona.cc", avatar.AvatarApi!.TokenIssuerAddress);
            Assert.Equal($"avatarapi-" + avatarName, avatar.AvatarApi!.ApiName);

            // cortex_library
            Assert.Equal($"https://fibona.cc/" + avatarName + "/cortex/neurons", avatar.CortexLibrary!.NeuronsUrl);
            Assert.Equal($"https://fibona.cc/" + avatarName + "/cortex/terminals", avatar.CortexLibrary!.TerminalsUrl);

            // un8y
            Assert.Equal("https://login.fibona.cc", avatar.Un8y!.OidcAuthorityUrl);
            Assert.Equal($"un8y-" + avatarName, avatar.Un8y!.ClientId);
            Assert.Equal($"/" + avatarName + "/un8y", avatar.Un8y!.BasePath);
            Assert.Equal(string.Empty, avatar.Un8y!.CertificatePassword);
            Assert.Equal("/https/aspnetapp.pfx", avatar.Un8y!.CertificatePath);

            // event sourcing
            Assert.Equal("/C/keys/private.key", avatar.EventSourcing!.PrivateKeyPath);
            Assert.Equal(string.Empty, avatar.EventSourcing!.InProcessPrivateKeyPath);
            Assert.False(avatar.EventSourcing!.EncryptionEnabled);
            Assert.Equal(string.Empty, avatar.EventSourcing!.EncryptedEventsKey);

            // orchestration
            Assert.Equal("192.168.1.110", avatar.Orchestration!.AvatarIp);
            Assert.Equal("192.168.1.110", avatar.Orchestration!.Un8yIp);
            Assert.Equal(64101, avatar.Orchestration!.AvatarInPort);
            Assert.Equal(64103, avatar.Orchestration!.Un8yBlazorPort);
            Assert.Equal(string.Empty, avatar.Orchestration!.KeysPath);
        }
    }
}