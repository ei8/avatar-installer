namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public interface IAvatarWriteRepository
    {
        Task Save(Avatar avatar, CancellationToken token = default);
    }
}
