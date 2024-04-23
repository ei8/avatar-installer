using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
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
        AssertionConcern.AssertArgumentNotNull(userRepository, nameof(userRepository));

        this.userRepository = userRepository;
    }

    public async Task AddAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        var u = await this.userRepository.GetByIdAsync(user.UserId);

        if (u is not null)
            throw new InvalidOperationException(string.Format(Constants.Messages.AlreadyExists, Constants.Titles.User));
        else
            await this.userRepository.SaveAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await this.userRepository.GetAllAsync();
    }

    public async Task RemoveAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        await this.userRepository.RemoveAsync(user);
    }

    public async Task SaveAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        var u = await this.userRepository.GetByIdAsync(user.UserId);

        if (u is null)
            throw new InvalidOperationException(string.Format(Constants.Messages.NotFound, Constants.TableNames.User));
        else
            await this.userRepository.SaveAsync(user);
    }
}
