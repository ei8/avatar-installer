using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

public partial class AvatarApiSettingsViewModel : BaseViewModel
{
    private readonly IAvatarSettingsApplicationService avatarSettingsApplicationService;

    public AvatarApiSettingsViewModel(IAvatarSettingsApplicationService avatarSettingsApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettingsApplicationService, nameof(avatarSettingsApplicationService));

        this.avatarSettingsApplicationService = avatarSettingsApplicationService;
    }

    [ObservableProperty]
    public string resourceDatabasePath;

    [ObservableProperty]
    public bool requireAuthentication;

    [ObservableProperty]
    public string anonymousUserId;

    [ObservableProperty]
    public string proxyUserId;

    [ObservableProperty]
    public string apiName;

    [ObservableProperty]
    public string apiSecret;

    [ObservableProperty]
    public string tokenIssuerUrl;

    [ObservableProperty]
    public bool validateServerCertificate;

    [RelayCommand]
    private async Task GetAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();
            var avatarApiSettings = avatarSettings.AvatarApi;

            this.ResourceDatabasePath = avatarApiSettings.ResourceDatabasePath;
            this.RequireAuthentication = avatarApiSettings.RequireAuthentication;
            this.AnonymousUserId = avatarApiSettings.AnonymousUserId.ToString();
            this.ProxyUserId = avatarApiSettings.ProxyUserId.ToString();
            this.TokenIssuerUrl = avatarApiSettings.TokenIssuerUrl;
            this.ApiName = avatarApiSettings.ApiName;
            this.ApiSecret = avatarApiSettings.ApiSecret;
            this.ValidateServerCertificate = avatarApiSettings.ValidateServerCertificate;
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

        if (!Guid.TryParse(this.AnonymousUserId, out var parsedAnonymousUserId))
        {
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.MustBe, nameof(this.AnonymousUserId), nameof(Guid))}",
                Constants.Prompts.Ok);

            return;
        }

        if (!Guid.TryParse(this.ProxyUserId, out var parsedProxyUserId))
        {
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.MustBe, nameof(this.ProxyUserId), nameof(Guid))}",
                Constants.Prompts.Ok);

            return;
        }

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Save,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.AvatarApiSettings),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();

            avatarSettings.AvatarApi.ResourceDatabasePath = this.ResourceDatabasePath;
            avatarSettings.AvatarApi.RequireAuthentication = this.RequireAuthentication;
            avatarSettings.AvatarApi.AnonymousUserId = parsedAnonymousUserId;
            avatarSettings.AvatarApi.ProxyUserId = parsedProxyUserId;
            avatarSettings.AvatarApi.TokenIssuerUrl = this.TokenIssuerUrl;
            avatarSettings.AvatarApi.ApiName = this.ApiName;
            avatarSettings.AvatarApi.ApiSecret = this.ApiSecret;
            avatarSettings.AvatarApi.ValidateServerCertificate = this.ValidateServerCertificate;

            await this.avatarSettingsApplicationService.SaveAsync(avatarSettings);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
              string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.AvatarApiSettings),
              Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.AvatarApiSettings)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;

        }
    }
}
