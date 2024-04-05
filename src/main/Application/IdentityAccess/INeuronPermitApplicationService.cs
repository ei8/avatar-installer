using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;

public interface INeuronPermitApplicationService
{
    Task<IEnumerable<NeuronPermit>> GetAllAsync();
    Task UpdateAsync(NeuronPermit neuronPermit);
}
