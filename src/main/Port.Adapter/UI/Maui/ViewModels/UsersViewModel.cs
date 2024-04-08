using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class UsersViewModel : EditAvatarViewModel
{
    public ObservableCollection<User> Users { get; set; } = [];
    private readonly IUserApplicationService userApplicationService;

    public UsersViewModel(IUserApplicationService userApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(userApplicationService, nameof(userApplicationService));

        this.userApplicationService = userApplicationService;
    }

    [RelayCommand]
    private async Task GetUsersAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            this.Users.Clear();
            var users = await this.userApplicationService.GetAllAsync();

            foreach (var user in users)
            {
                this.Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, 
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Get, Constants.Titles.User)}s: {ex.Message}", 
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToUserDetailsAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        await Shell.Current.GoToAsync($"{nameof(UserDetailsPage)}",
            new Dictionary<string, object>
            {
                { "User", user }
            });
    }
}
