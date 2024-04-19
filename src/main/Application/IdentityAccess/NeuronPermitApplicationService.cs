using ei8.Avatar.Installer.Common;
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

    public async Task AddAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        var np = await this.neuronPermitRepository.GetByCompositeIdAsync(neuronPermit.UserNeuronId, neuronPermit.NeuronId);

        if (np is not null) 
            throw new InvalidOperationException(string.Format(Constants.Messages.AlreadyExists, Constants.Titles.NeuronPermit));
        else 
            await this.neuronPermitRepository.SaveAsync(neuronPermit);
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

        var np = await this.neuronPermitRepository.GetByCompositeIdAsync(neuronPermit.UserNeuronId, neuronPermit.NeuronId);

        if (np is null)
            throw new InvalidOperationException(string.Format(Constants.Messages.NotFound, Constants.Titles.NeuronPermit));
        else
            await this.neuronPermitRepository.SaveAsync(neuronPermit);
    }
}
