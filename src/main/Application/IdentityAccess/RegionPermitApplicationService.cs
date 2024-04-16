﻿using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
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

    public async Task<IEnumerable<RegionPermit>> GetAllAsync()
    {
        return await this.regionPermitRepository.GetAllAsync();
    }

    public async Task RemoveAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        await this.regionPermitRepository.RemoveAsync(regionPermit);
    }

    public async Task SaveAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        await this.regionPermitRepository.SaveAsync(regionPermit);
    }
}
