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

    public async Task<bool> CheckIfExistsAsync(string userNeuronId, string neuronId)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermitRepository, nameof(neuronPermitRepository));
        AssertionConcern.AssertArgumentNotNull(neuronPermitRepository, nameof(neuronPermitRepository));

        var neuronPermit = await neuronPermitRepository.GetByIdAsync(userNeuronId, neuronId);
        return neuronPermit is not null;
    }

    public async Task<IEnumerable<NeuronPermit>> GetAllAsync()
    {
        return await this.neuronPermitRepository.GetAllAsync();
    }

    public async Task RemoveAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        await this.neuronPermitRepository.RemoveAsync(neuronPermit);
    }

    public async Task SaveAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        await this.neuronPermitRepository.SaveAsync(neuronPermit);
    }
}
