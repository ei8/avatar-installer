using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;
public interface IUserApplicationService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task AddAsync(User user);
}
