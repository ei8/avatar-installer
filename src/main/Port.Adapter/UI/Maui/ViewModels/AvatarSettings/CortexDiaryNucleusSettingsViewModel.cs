using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using neurUL.Common.Domain.Model;
using System.Diagnostics;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

public partial class CortexDiaryNucleusSettingsViewModel : BaseViewModel
{
    private readonly IAvatarSettingsApplicationService avatarSettingsApplicationService;

    public CortexDiaryNucleusSettingsViewModel(IAvatarSettingsApplicationService avatarSettingsApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettingsApplicationService, nameof(avatarSettingsApplicationService));

        this.avatarSettingsApplicationService = avatarSettingsApplicationService;
    }

    [ObservableProperty]
    public string subscriptionsDatabasePath;

    [ObservableProperty]
    public int subscriptionsPollingIntervalSecs;

    [ObservableProperty]
    public string subscriptionsPushOwner;

    [ObservableProperty]
    public string subscriptionsPushPublicKey;

    [ObservableProperty]
    public string subscriptionsPushPrivateKey;

    [ObservableProperty]
    public string subscriptionsSmtpServerAddress;

    [ObservableProperty]
    public int subscriptionsSmtpPort;

    [ObservableProperty]
    public bool subscriptionsSmtpUseSsl;

    [ObservableProperty]
    public string subscriptionsSmtpSenderName;

    [ObservableProperty]
    public string subscriptionsSmtpSenderAddress;

    [ObservableProperty]
    public string subscriptionsSmtpSenderUsername;

    [ObservableProperty]
    public string subscriptionsSmtpSenderPassword;

    [ObservableProperty]
    public string subscriptionsCortexGraphOutBaseUrl;

    [RelayCommand]
    private async Task GetAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();
            var cortexDiaryNucleusSettings = avatarSettings.CortexDiaryNucleus;

            this.SubscriptionsDatabasePath = cortexDiaryNucleusSettings.SubscriptionsDatabasePath;
            this.SubscriptionsPollingIntervalSecs = cortexDiaryNucleusSettings.SubscriptionsPollingIntervalSecs;
            this.SubscriptionsPushOwner = cortexDiaryNucleusSettings.SubscriptionsPushOwner;
            this.SubscriptionsPushPublicKey = cortexDiaryNucleusSettings.SubscriptionsPushPublicKey;
            this.SubscriptionsPushPrivateKey = cortexDiaryNucleusSettings.SubscriptionsPushPrivateKey;
            this.SubscriptionsSmtpServerAddress = cortexDiaryNucleusSettings.SubscriptionsSmtpServerAddress;
            this.SubscriptionsSmtpPort = cortexDiaryNucleusSettings.SubscriptionsSmtpPort;
            this.SubscriptionsSmtpUseSsl = cortexDiaryNucleusSettings.SubscriptionsSmtpUseSsl;
            this.SubscriptionsSmtpSenderName = cortexDiaryNucleusSettings.SubscriptionsSmtpSenderName;
            this.SubscriptionsSmtpSenderAddress = cortexDiaryNucleusSettings.SubscriptionsSmtpSenderAddress;
            this.SubscriptionsSmtpSenderUsername = cortexDiaryNucleusSettings.SubscriptionsSmtpSenderUsername;
            this.SubscriptionsSmtpSenderPassword = cortexDiaryNucleusSettings.SubscriptionsSmtpSenderPassword;
            this.SubscriptionsCortexGraphOutBaseUrl = cortexDiaryNucleusSettings.SubscriptionsCortexGraphOutBaseUrl;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Get, Constants.Titles.CortexDiaryNucleusSettings)}: {ex.Message}",
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
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.CortexDiaryNucleusSettings),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();

            avatarSettings.CortexDiaryNucleus.SubscriptionsDatabasePath = this.SubscriptionsDatabasePath;
            avatarSettings.CortexDiaryNucleus.SubscriptionsPollingIntervalSecs = this.SubscriptionsPollingIntervalSecs;
            avatarSettings.CortexDiaryNucleus.SubscriptionsPushOwner = this.SubscriptionsPushOwner;
            avatarSettings.CortexDiaryNucleus.SubscriptionsPushPublicKey = this.SubscriptionsPushPublicKey;
            avatarSettings.CortexDiaryNucleus.SubscriptionsPushPrivateKey = this.SubscriptionsPushPrivateKey;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpServerAddress = this.SubscriptionsSmtpServerAddress;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpPort = this.SubscriptionsSmtpPort;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpUseSsl = this.SubscriptionsSmtpUseSsl;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderName = this.SubscriptionsSmtpSenderName;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderAddress = this.SubscriptionsSmtpSenderAddress;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderUsername = this.SubscriptionsSmtpSenderUsername;
            avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderPassword = this.SubscriptionsSmtpSenderPassword;
            avatarSettings.CortexDiaryNucleus.SubscriptionsCortexGraphOutBaseUrl = this.SubscriptionsCortexGraphOutBaseUrl;

            await this.avatarSettingsApplicationService.SaveAsync(avatarSettings);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
              string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.CortexDiaryNucleusSettings),
              Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.CortexDiaryNucleusSettings)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
