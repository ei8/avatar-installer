using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ei8.Avatar.Installer.Domain.Model.Avatars;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarRepository : IAvatarRepository
    {
        public Task<AvatarItem> GetByAsync(string id)
        {
            // source of truth - local filesystem
            // read from filesystem and create AvatarItem instance
            return Task.FromResult(new AvatarItem());
        }

        public Task SaveAsync(AvatarItem avatarItem)
        {
            // TODO: commit to filesystem

            // TODO: generate the following files
            // sql scripts for sqlite databases (save and execute to create dbs)
            // docker-compose files
            throw new NotImplementedException();
        }
    }
}
