﻿using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;
public interface IRegionPermitApplicationService
{
    Task<IEnumerable<RegionPermit>> GetAllAsync();
    Task UpdateAsync(RegionPermit regionPermit);
    Task DeleteAsync(RegionPermit regionPermit);
    Task AddAsync(RegionPermit regionPermit);
}
