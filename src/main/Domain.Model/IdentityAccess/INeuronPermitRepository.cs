using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface INeuronPermitRepository
{
    Task<IEnumerable<NeuronPermit>> GetNeuronPermitsAsync(string id);
    //Task<IEnumerable<NeuronPermit>> GetAllAsync();

    Task UpdateNeuronPermitAsync(string id, NeuronPermit neuronPermit);
    //Task UpdateAsync(NeuronPermit neuronPermit);
}
