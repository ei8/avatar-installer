using ei8.Avatar.Installer.Domain.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface INeuronPermitRepository
{
    Task<IEnumerable<NeuronPermitDto>> GetNeuronPermitsAsync(string access);
}
