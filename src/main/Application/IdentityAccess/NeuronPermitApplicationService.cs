using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;
public class NeuronPermitApplicationService : INeuronPermitApplicationService
{
    private readonly INeuronPermitRepository neuronPermitRepository;

    public NeuronPermitApplicationService(INeuronPermitRepository neuronPermitRepository)
    {
        this.neuronPermitRepository = neuronPermitRepository;
    }

    public async Task<IEnumerable<NeuronPermit>> GetAllAsync()
    {
        return await this.neuronPermitRepository.GetAllAsync();
    }

    public async Task UpdateAsync(NeuronPermit neuronPermit)
    {
        await this.neuronPermitRepository.UpdateAsync(neuronPermit);
    }
}
