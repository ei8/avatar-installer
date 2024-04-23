using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.IdentityAccess;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task RemoveAsync(User user);
    Task SaveAsync(User user);
}
