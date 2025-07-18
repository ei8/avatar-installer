﻿using AutoMapper;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using ei8.Avatar.Installer.Domain.Model.Configuration;

namespace ei8.Avatar.Installer.Domain.Model.Mapping
{
    public class AvatarMapperService : IAvatarMapperService
    {
        private readonly IMapper mapper;

        public AvatarMapperService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public AvatarItem Apply(AvatarConfigurationItem configItem, AvatarItem avatarItem)
        {
            return mapper.Map(configItem, avatarItem);
        }
    }

    public class AvatarAutoMapperProfile : Profile
    {
        public AvatarAutoMapperProfile()
        {
            CreateMap<AvatarConfigurationItem, AvatarItem>()
                .ForPath(
                    dest => dest.Settings.CortexGraphPersistence,
                    opt => opt.MapFrom(src => src.CortexGraphPersistence)
                )
                .ForPath(
                    dest => dest.Settings.CortexGraph,
                    opt => opt.MapFrom(src => src.CortexGraph)
                )
                .ForPath(
                    dest => dest.Settings.AvatarApi, 
                    opt => opt.MapFrom(src => src.AvatarApi)
                )
                .ForPath(
                    dest => dest.Settings.CortexLibrary,
                    opt => opt.MapFrom(src => src.CortexLibrary)
                )
                .ForPath(
                    dest => dest.Settings.CortexChatNucleus,
                    opt => opt.MapFrom(src => src.CortexChatNucleus)
                );

            CreateMap<CortexGraphPersistenceConfiguration, CortexGraphPersistenceSettings>();
            CreateMap<CortexGraphConfiguration, CortexGraphSettings>();
            CreateMap<AvatarApiConfiguration, AvatarApiSettings>();
            CreateMap<CortexLibraryConfiguration, CortexLibrarySettings>();
            CreateMap<Un8yConfiguration, Un8ySettings>()
                .ForMember(
                    dest => dest.OidcAuthority,
                    opt => opt.MapFrom(src => src.OidcAuthorityUrl)
                );

            CreateMap<NetworkConfiguration, AvatarNetworkSettings>()
                .ForMember(dest => dest.AvatarIp, opt => opt.MapFrom(src => src.LocalIp))
                .ForMember(dest => dest.Un8yIp, opt => opt.MapFrom(src => src.LocalIp));

            CreateMap<CortexChatNucleusConfiguration, CortexChatNucleusSettings>();
        }
    }
}
