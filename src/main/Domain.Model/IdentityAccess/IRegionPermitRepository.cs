using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface IRegionPermitRepository
{
    Task<IEnumerable<RegionPermit>> GetAllAsync();
    Task<RegionPermit> GetByIdAsync(int sequenceId);
    Task RemoveAsync(RegionPermit regionPermit);
    Task SaveAsync(RegionPermit regionPermit);
}
