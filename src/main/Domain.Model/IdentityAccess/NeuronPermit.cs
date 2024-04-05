using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public class NeuronPermit
{
    public string UserNeuronId { get; set; } = string.Empty;
    public string NeuronId { get; set; } = string.Empty;
    public string ExpirationDate { get; set; }
}
