namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarRepository
    {
        /// <summary>
        /// Writes the values of the <see cref="AvatarItem"/> into the record with the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatarItem"></param>
        /// <returns></returns>
        Task SaveAsync(string id, AvatarItem avatarItem);

        /// <summary>
        /// Retrieves an instance of a <see cref="AvatarItem"/> with the specified identifier.
        /// </summary>
        /// <param name="id">Name of the avatar</param>
        /// <returns></returns>
        Task<AvatarItem?> GetByAsync(string id);
    }
}
