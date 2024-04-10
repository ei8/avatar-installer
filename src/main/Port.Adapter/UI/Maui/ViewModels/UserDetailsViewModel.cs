using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;
using neurUL.Common.Domain.Model;
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
        AssertionConcern.AssertArgumentNotNull(userApplicationService, nameof(userApplicationService));

        this.userApplicationService = userApplicationService;
    }

    [ObservableProperty]
    private User user;

    [RelayCommand]
    private async Task UpdateUserAsync()
    {
        if (this.User is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Update,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Update, Constants.Titles.User),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            await this.userApplicationService.UpdateAsync(User);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Updated, Constants.Titles.User),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Update, Constants.Titles.User)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
    }

    [RelayCommand]
    private async Task DeleteUserAsync()
    {
        if (this.User is null) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Delete,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Delete, Constants.Titles.User),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            await this.userApplicationService.DeleteAsync(User);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Deleted, Constants.Titles.User),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Delete, Constants.Titles.User)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
    }
}
