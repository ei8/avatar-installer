using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface INeuronPermitRepository
{
    Task<IEnumerable<NeuronPermit>> GetAllAsync(string id);
    Task UpdateAsync(string id, NeuronPermit neuronPermit);
}
