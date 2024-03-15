using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
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
        this.regionPermitRepository = regionPermitRepository;
    }

    public async Task<IEnumerable<RegionPermit>> GetAllAsync()
    {
        return await this.regionPermitRepository.GetAllAsync();
    }

    public async Task UpdateAsync(RegionPermit regionPermit)
    {
        await this.regionPermitRepository.UpdateAsync(regionPermit);
    }
}
