﻿using AutoMapper;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;

namespace Domain.Model.Test.Mapping
{
    public class AvatarMapperServiceTests
    {
        private readonly MapperConfiguration mapperConfig;
        private readonly IMapper mapper;

        // setup
        public AvatarMapperServiceTests()
        {
            this.mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AvatarAutoMapperProfile>();
            });

            this.mapper = new Mapper(mapperConfig);
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
                            ArangoRootPassword = "test",
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

            Assert.Equal("test", sampleResult.Settings.CortexGraph.ArangoRootPassword);
            Assert.Equal("custom", sampleResult.Settings.CortexGraph.DbName);
            Assert.Equal(@"http://www.example.com", sampleResult.Settings.CortexGraph.DbUrl);
            Assert.Equal("not-root", sampleResult.Settings.CortexGraph.DbUsername);

            var defaultTarget = new AvatarItem("id_defaults1", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("", defaultResult.Settings.CortexGraph.ArangoRootPassword);
            Assert.Equal("graph", defaultResult.Settings.CortexGraph.DbName);
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
                            TokenIssuerUrl = @"https://www.junvic.me"
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample2", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal("api", sampleResult.Settings.AvatarApi.ApiName);
            Assert.Equal(@"https://www.junvic.me", sampleResult.Settings.AvatarApi.TokenIssuerUrl);


            var defaultTarget = new AvatarItem("id_defaults2", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("avatarapi-sample", defaultResult.Settings.AvatarApi.ApiName);
            Assert.Equal(@"https://login.fibona.cc", defaultResult.Settings.AvatarApi.TokenIssuerUrl);
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

            Assert.Equal(@"https://fibona.cc/sample/cortex/neurons", defaultResult.Settings.CortexLibrary.NeuronsUrl);
            Assert.Equal(@"https://fibona.cc/sample/cortex/terminals", defaultResult.Settings.CortexLibrary.TerminalsUrl);
        }

        [Fact]
        public void MapsD23Configuration()
        {
            var config = new AvatarConfiguration
            {
                Avatars = new AvatarConfigurationItem[2]
                {
                    new("sample")
                    {
                        D23 = new()
                        {
                            OidcAuthorityUrl = @"https://www.example.com",
                            ClientId = "not-sample",
                            BasePath = "/"
                        }
                    },

                    new("defaults")
                }
            };

            var sut = new AvatarMapperService(mapper);
            var sampleTarget = new AvatarItem("id_sample4", "sample");
            var sampleResult = sut.Apply(config.Avatars[0], sampleTarget);

            Assert.Equal(@"https://www.example.com", sampleResult.d23.OidcAuthority);
            Assert.Equal("not-sample", sampleResult.d23.ClientId);
            Assert.Equal("/", sampleResult.d23.BasePath);


            var defaultTarget = new AvatarItem("id_defaults4", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal(@"https://login.fibona.cc", defaultResult.d23.OidcAuthority);
            Assert.Equal("d23-sample", defaultResult.d23.ClientId);
            Assert.Equal("/sample/d23", defaultResult.d23.BasePath);
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
                            d23BlazorPort = 67890,
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
            Assert.Equal("127.0.0.1", sampleResult.Network.D23Ip);
            Assert.Equal(12345, sampleResult.Network.AvatarInPort);
            Assert.Equal(67890, sampleResult.Network.D23BlazorPort);


            var defaultTarget = new AvatarItem("id_defaults5", "defaults");
            var defaultResult = sut.Apply(config.Avatars[1], defaultTarget);

            Assert.Equal("192.168.1.110", defaultResult.Network.AvatarIp);
            Assert.Equal("192.168.1.110", defaultResult.Network.D23Ip);
            Assert.Equal(64101, defaultResult.Network.AvatarInPort);
            Assert.Equal(64103, defaultResult.Network.D23BlazorPort);
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
                        AnonymousUserId = Guid.Parse("498c5d30-1253-4baf-8bb5-0af6c3d66a91"),
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
            Assert.Equal(Guid.Parse("498c5d30-1253-4baf-8bb5-0af6c3d66a91"), sampleResult.Settings.AvatarApi.AnonymousUserId);
            Assert.Equal("/C/db/identity-access.db", sampleResult.Settings.IdentityAccess.UserDatabasePath);
            Assert.Equal("BLrW80tp5imbJlxAY5WJnmtzaZvTCJoM8ywZEI6E65VTHcqtB69tnqUsRkYC6U-1WfSj0bFovZF6DZaA9Bgo0Ts", sampleResult.Settings.CortexDiaryNucleus.SubscriptionsPushPublicKey);
        }
    }
}
