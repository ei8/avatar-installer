using ei8.Avatar.Installer.Domain.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface INeuronPermitRepository
{
    Task<IEnumerable<NeuronPermit>> GetNeuronPermitsAsync(string connection);
    Task UpdateNeuronPermitAsync(string access, NeuronPermit neuronPermit);
}
