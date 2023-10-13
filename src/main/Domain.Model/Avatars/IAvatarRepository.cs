namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarRepository
    {
        Task SaveAsync(AvatarItem avatarItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Name of the avatar</param>
        /// <returns></returns>
        Task<AvatarItem?> GetByAsync(string id);
    }
}
