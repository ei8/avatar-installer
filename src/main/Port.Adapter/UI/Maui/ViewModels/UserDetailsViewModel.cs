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

[QueryProperty(nameof(User), "User")]
[QueryProperty(nameof(Mode), "Mode")]
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditing))]
    private Mode mode;

    public bool IsEditing => this.Mode == Mode.Edit;

    [RelayCommand]
    private async Task SaveUserAsync()
    {
        if (this.User is null || this.IsBusy) return;

        if (string.IsNullOrEmpty(this.User.UserId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBe, nameof(this.User.UserId), Constants.Operations.Empty), 
                Constants.Prompts.Ok);
            return;
        }

        if (string.IsNullOrEmpty(this.User.NeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBe, nameof(this.User.NeuronId), Constants.Operations.Empty),
                Constants.Prompts.Ok);
            return;
        }

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Save,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.User),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            if (this.Mode == Mode.Create)
            {
                var exists = await this.userApplicationService.CheckIfExistsAsync(this.User.UserId);

                if (exists)
                {
                    await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                        string.Format(Constants.Messages.AlreadyExists, Constants.Titles.User),
                        Constants.Prompts.Ok);
                    return;
                }
            }
            await this.userApplicationService.SaveAsync(this.User);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.User),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.User)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveUserAsync()
    {
        if (this.User is null || this.IsBusy) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Remove,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Remove, Constants.Titles.User),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            await this.userApplicationService.RemoveAsync(this.User);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Removed, Constants.Titles.User),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Remove, Constants.Titles.User)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
