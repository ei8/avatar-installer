using AutoMapper;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using static System.Formats.Asn1.AsnWriter;

namespace Domain.Model.Test.Mapping
{
    public class AvatarMapperServiceFixture
    {
        private readonly MapperConfiguration mapperConfig;
        private readonly IMapper mapper;

        // setup
        public AvatarMapperServiceFixture()
        {
            this.mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AvatarAutoMapperProfile>();
            });

            this.mapper = new Mapper(mapperConfig);
        }

        [Fact]
        public void MapsCortexGraphPersistenceConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
            new("sample")
            {
                CortexGraphPersistence = new()
                {
                    ArangoRootPassword = "test",
                }
            },
            new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);

            var sampleTarget = new AvatarItem("id_sample1", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("test", sampleResult.Settings.CortexGraphPersistence.ArangoRootPassword);

            var defaultTarget = new AvatarItem("id_defaults1", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("", defaultResult.Settings.CortexGraphPersistence.ArangoRootPassword);
        }

        [Fact]
        public void MapsCortexGraphConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        CortexGraph = new()
                        {
                            DbName = "custom",
                            DbUrl = @"http://www.example.com",
                            DbUsername = "not-root"
                        }
                    },
                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);

            var sampleTarget = new AvatarItem("id_sample1", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("custom", sampleResult.Settings.CortexGraph.DbName);
            Assert.Equal(@"http://www.example.com", sampleResult.Settings.CortexGraph.DbUrl);
            Assert.Equal("not-root", sampleResult.Settings.CortexGraph.DbUsername);

            var defaultTarget = new AvatarItem("id_defaults1", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("graph_defaults", defaultResult.Settings.CortexGraph.DbName);
            Assert.Equal(@"http://cortex.graph.persistence:8529", defaultResult.Settings.CortexGraph.DbUrl);
            Assert.Equal("root", defaultResult.Settings.CortexGraph.DbUsername);
        }

        [Fact]
        public void MapsAvatarApiConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        AvatarApi = new()
                        {
                            ApiName = "api",
                            TokenIssuerAddress = @"https://www.junvic.me",
                            AnonymousUserId = "Guestotototto"
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample2", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("api", sampleResult.Settings.AvatarApi.ApiName);
            Assert.Equal(@"https://www.junvic.me", sampleResult.Settings.AvatarApi.TokenIssuerAddress);
            Assert.Equal("Guestotototto", sampleResult.Settings.AvatarApi.AnonymousUserId);


            var defaultTarget = new AvatarItem("id_defaults2", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("avatarapi-defaults", defaultResult.Settings.AvatarApi.ApiName);
            Assert.Equal(@"https://login.fibona.cc", defaultResult.Settings.AvatarApi.TokenIssuerAddress);
        }

        [Fact]
        public void MapsCortexLibraryConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        CortexLibrary = new()
                        {
                            NeuronsUrl = @"https://neuronsurl.example.com",
                            TerminalsUrl = @"https://terminalsurl.example.com"
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample3", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal(@"https://neuronsurl.example.com", sampleResult.Settings.CortexLibrary.NeuronsUrl);
            Assert.Equal(@"https://terminalsurl.example.com", sampleResult.Settings.CortexLibrary.TerminalsUrl);


            var defaultTarget = new AvatarItem("id_defaults3", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal(@"https://fibona.cc/defaults/cortex/neurons", defaultResult.Settings.CortexLibrary.NeuronsUrl);
            Assert.Equal(@"https://fibona.cc/defaults/cortex/terminals", defaultResult.Settings.CortexLibrary.TerminalsUrl);
        }

        [Fact]
        public void MapsUn8yConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        Un8y = new()
                        {
                            OidcAuthorityUrl = @"https://www.example.com",
                            ClientId = "not-sample",
                            RequestedScopes = "new scope",
                            BasePath = "/"
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample4", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal(@"https://www.example.com", sampleResult.Un8y.OidcAuthority);
            Assert.Equal("not-sample", sampleResult.Un8y.ClientId);
            Assert.Equal("new scope", sampleResult.Un8y.RequestedScopes);
            Assert.Equal("/", sampleResult.Un8y.BasePath);


            var defaultTarget = new AvatarItem("id_defaults4", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal(@"https://login.fibona.cc", defaultResult.Un8y.OidcAuthority);
            Assert.Equal("un8y-defaults", defaultResult.Un8y.ClientId);
            Assert.Equal($"openid,profile,email,avatarapi-defaults,offline_access", defaultResult.Un8y.RequestedScopes);
            Assert.Equal("/defaults/un8y", defaultResult.Un8y.BasePath);
        }

        [Fact]
        public void MapsNetworkConfiguration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        Network = new()
                        {
                            AvatarInPort = 12345,
                            Un8yBlazorPort = 67890,
                            LocalIp = "127.0.0.1",
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample5", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("127.0.0.1", sampleResult.Network.AvatarIp);
            Assert.Equal("127.0.0.1", sampleResult.Network.Un8yIp);
            Assert.Equal(12345, sampleResult.Network.AvatarInPort);
            Assert.Equal(67890, sampleResult.Network.Un8yBlazorPort);


            var defaultTarget = new AvatarItem("id_defaults5", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("192.168.1.110", defaultResult.Network.AvatarIp);
            Assert.Equal("192.168.1.110", defaultResult.Network.Un8yIp);
            Assert.Equal(64101, defaultResult.Network.AvatarInPort);
            Assert.Equal(64103, defaultResult.Network.Un8yBlazorPort);
        }

        [Fact]
        public void DoesNotOverwritePropertiesNotConfigured()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[1]
                {
                    new("sample")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample6", "sample")
            {
                Settings = new AvatarSettings
                {
                    EventSourcing = new EventSourcingSettings()
                    {
                        DatabasePath = "/C/db/events.db",
                        DisplayErrorTraces = false
                    },
                    CortexGraph = new CortexGraphSettings()
                    {
                        PollInterval = 1000,
                        DefaultRelativeValues = 3,
                        DefaultNeuronActiveValues = 1,
                        DefaultTerminalActiveValues = 1,
                        DefaultPage = 1,
                        DefaultPageSize = 10
                    },
                    AvatarApi = new AvatarApiSettings()
                    {
                        ResourceDatabasePath = "/C/db/avatar.db",
                        RequireAuthentication = true,
                        AnonymousUserId = "Gusto",
                        ProxyUserId = Guid.Parse("ca008cde-61bb-4260-92fb-9abcca1209ef"),
                        ApiSecret = "72019790-7daa-4de6-a7a9-3a1615cabf59",
                        ValidateServerCertificate = false
                    },
                    IdentityAccess = new IdentityAccessSettings()
                    {
                        UserDatabasePath = "/C/db/identity-access.db"
                    },
                    CortexDiaryNucleus = new CortexDiaryNucleusSettings()
                    {
                        SubscriptionsDatabasePath = "/C/db/subscriptions.db",
                        SubscriptionsPollingIntervalSecs = 15,
                        SubscriptionsPushOwner = "mailto:example@example.com",
                        SubscriptionsPushPublicKey = "BLrW80tp5imbJlxAY5WJnmtzaZvTCJoM8ywZEI6E65VTHcqtB69tnqUsRkYC6U-1WfSj0bFovZF6DZaA9Bgo0Ts",
                        SubscriptionsPushPrivateKey = "JlI0-oL2NQ8HyAAZF3pGxWtbjRsYQxTB9tg6Fe7_1x8",
                        SubscriptionsSmtpServerAddress = "smtp.gmail.com",
                        SubscriptionsSmtpPort = 587,
                        SubscriptionsSmtpUseSsl = false,
                        SubscriptionsSmtpSenderName = "ei8 Support",
                        SubscriptionsSmtpSenderAddress = "support@ei8.works",
                        SubscriptionsSmtpSenderUsername = "support@ei8.works",
                        SubscriptionsSmtpSenderPassword = string.Empty,
                        SubscriptionsCortexGraphOutBaseUrl = "http://cortex.graph.out.api:80"
                    }
                }
            };

            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("/C/db/events.db", sampleResult.Settings.EventSourcing.DatabasePath);
            Assert.Equal(1000, sampleResult.Settings.CortexGraph.PollInterval);
            Assert.Equal("Guest", sampleResult.Settings.AvatarApi.AnonymousUserId);
            Assert.Equal("/C/db/identity-access.db", sampleResult.Settings.IdentityAccess.UserDatabasePath);
            Assert.Equal("BLrW80tp5imbJlxAY5WJnmtzaZvTCJoM8ywZEI6E65VTHcqtB69tnqUsRkYC6U-1WfSj0bFovZF6DZaA9Bgo0Ts", sampleResult.Settings.CortexDiaryNucleus.SubscriptionsPushPublicKey);
        }
    }
}
