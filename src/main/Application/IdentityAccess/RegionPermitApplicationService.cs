using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;
public class RegionPermitApplicationService : IRegionPermitApplicationService
{
    private readonly IRegionPermitRepository regionPermitRepository;

    public RegionPermitApplicationService(IRegionPermitRepository regionPermitRepository)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermitRepository, nameof(regionPermitRepository));
        
        this.regionPermitRepository = regionPermitRepository;
    }

    public async Task DeleteAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        await this.regionPermitRepository.DeleteAsync(regionPermit);
    }

    public async Task<IEnumerable<RegionPermit>> GetAllAsync()
    {
        return await this.regionPermitRepository.GetAllAsync();
    }

    public async Task UpdateAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        await this.regionPermitRepository.UpdateAsync(regionPermit);
    }
}
