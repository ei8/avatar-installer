using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;

namespace ei8.Avatar.Installer.Domain.Model.Mapping
{
    public interface IAvatarMapperService
    {
        AvatarItem Apply(AvatarConfigurationItem configItem, AvatarItem avatarItem);
    }
}
