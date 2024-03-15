using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.IdentityAccess;

public class UserApplicationService : IUserApplicationService
{
    private readonly IUserRepository userRepository;

    public UserApplicationService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await this.userRepository.GetAllAsync();
    }

    public async Task UpdateAsync(User user)
    {
        await this.userRepository.UpdateAsync(user);
    }
}
