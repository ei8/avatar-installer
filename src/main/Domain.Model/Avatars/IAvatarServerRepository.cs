namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarServerRepository
    {
        Task<AvatarServer?> GetByAsync(string id);
        Task SaveAsync(string id, AvatarServer avatarServer);
    }
}
