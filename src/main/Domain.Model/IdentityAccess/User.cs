using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public class User
{
    public string UserId { get; set; }
    public string NeuronId { get; set; }
    public int? Active { get; set; }
}
