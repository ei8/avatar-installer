namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarItemWriteRepository
    {
        /// <summary>
        /// Writes the values of the <see cref="AvatarItem"/> into the record with the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatarItem"></param>
        /// <returns></returns>
        Task SaveAsync(AvatarItem avatarItem);
    }
}
