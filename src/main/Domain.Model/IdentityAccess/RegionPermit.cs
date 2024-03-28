using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public class RegionPermit
{
    public int SequenceId { get; set; }
    public string UserNeuronId { get; set; }
    public string RegionNeuronId { get; set; }
    public int? WriteLevel { get; set; }
    public int? ReadLevel { get; set; }
}
