using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
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
        AssertionConcern.AssertArgumentNotNull(neuronPermitRepository, nameof(neuronPermitRepository));

        this.neuronPermitRepository = neuronPermitRepository;
    }

    public async Task<IEnumerable<NeuronPermit>> GetAllAsync()
    {
        return await this.neuronPermitRepository.GetAllAsync();
    }

    public async Task UpdateAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        await this.neuronPermitRepository.UpdateAsync(neuronPermit);
    }
}
