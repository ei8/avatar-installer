using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class AvatarNetworkSettings
{
    public string AvatarIp { get; set; }
    public string D23Ip { get; set; }
    public int AvatarInPort { get; set; }
    public int D23BlazorPort { get; set; }
}
