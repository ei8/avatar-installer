using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public class NeuronPermit
{
    public string UserNeuronId { get; set; }
    public string NeuronId { get; set; }
    public string ExpirationDate { get; set; }
}
