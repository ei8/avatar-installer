using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

[QueryProperty("User", "User")]
public partial class UserDetailsViewModel : EditAvatarViewModel
{
    private readonly IUserApplicationService userApplicationService;

    public UserDetailsViewModel(IUserApplicationService userApplicationService)
    {
        this.userApplicationService = userApplicationService;
    }

    [ObservableProperty]
    private User? user;

    [RelayCommand]
    private async Task UpdateUserAsync()
    {
        if (User is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update User", "Are you sure you want to update this User?",
                            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            await this.userApplicationService.UpdateAsync(User!);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                $"User updated", Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"Unable to update User: {ex.Message}", Constants.Prompts.Ok);
        }
    }
}
