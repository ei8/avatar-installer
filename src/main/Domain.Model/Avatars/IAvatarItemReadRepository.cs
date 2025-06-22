namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarItemReadRepository
    {
        /// <summary>
        /// Retrieves an instance of a <see cref="AvatarItem"/> with the specified identifier.
        /// </summary>
        /// <param name="id">Name of the avatar</param>
        /// <returns></returns>
        Task<AvatarItem> GetByAsync(string id);
    }
}
