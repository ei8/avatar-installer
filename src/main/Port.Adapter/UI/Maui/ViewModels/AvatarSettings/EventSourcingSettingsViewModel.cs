using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

public partial class EventSourcingSettingsViewModel : EditAvatarViewModel
{
    private readonly IAvatarSettingsApplicationService avatarSettingsApplicationService;

    public EventSourcingSettingsViewModel(IAvatarSettingsApplicationService avatarSettingsApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettingsApplicationService, nameof(avatarSettingsApplicationService));

        this.avatarSettingsApplicationService = avatarSettingsApplicationService;
    }

    [ObservableProperty]
    public string databasePath;

    [ObservableProperty]
    public bool displayErrorTraces;

    [RelayCommand]
    private async Task GetAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();
            var eventSourcingSettings = avatarSettings.EventSourcing;

            this.DatabasePath = eventSourcingSettings.DatabasePath;
            this.DisplayErrorTraces = eventSourcingSettings.DisplayErrorTraces;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Get, Constants.Titles.EventSourcingSettings)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (this.IsBusy)
            return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Save,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.EventSourcingSettings),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();
            avatarSettings.EventSourcing.DatabasePath = this.DatabasePath;
            avatarSettings.EventSourcing.DisplayErrorTraces = this.DisplayErrorTraces;

            await this.avatarSettingsApplicationService.SaveAsync(avatarSettings);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
              string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.EventSourcingSettings),
              Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.EventSourcingSettings)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
