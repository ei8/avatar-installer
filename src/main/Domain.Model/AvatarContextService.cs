using ei8.Avatar.Installer.Domain.Model.Avatars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model;

public class AvatarContextService : IAvatarContextService
{
    public AvatarItem? Avatar { get; set; }
}
