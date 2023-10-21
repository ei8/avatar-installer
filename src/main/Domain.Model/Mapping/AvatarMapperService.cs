using AutoMapper;
using ei8.Avatar.Installer.Domain.Model.Avatars;
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
            CreateMap<AvatarConfigurationItem, AvatarItem>();
            CreateMap<CortexGraphConfiguration, CortexGraphSettings>();
            CreateMap<AvatarApiConfiguration, AvatarApiSettings>();
            CreateMap<CortexLibraryConfiguration, CortexLibrarySettings>();
            CreateMap<d23Configuration, d23Settings>()
                .ForMember(dest => dest.OidcAuthority, opt => opt.MapFrom(src => src.OidcAuthorityUrl));

            CreateMap<NetworkConfiguration, AvatarNetworkSettings>()
                .ForMember(dest => dest.AvatarIp, opt => opt.MapFrom(src => src.LocalIp))
                .ForMember(dest => dest.D23Ip, opt => opt.MapFrom(src => src.LocalIp));
        }
    }
}
