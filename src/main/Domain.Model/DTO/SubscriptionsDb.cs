using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public class AvatarDto
{
    public string Hash { get; set; }
    public string Url { get; set; }
    public string Id { get; set; }
}

public class BrowserReceiverDto
{
    public string Id { get; set; }
    public string UserNeuronId { get; set; }
    public string Name { get; set; }
    public string PushEndpoint { get; set; }
    public string PushP256DH { get; set; }
    public string PushAuth { get; set; }
}

public class SmtpReceiverDto
{
    public string Id { get; set; }
    public string UserNeuronId { get; set; }
    public string AvatarId { get; set; }
}

//public class UserDto
//{
//    public string UserNeuronId { get; set; }
//}
