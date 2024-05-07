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

public partial class CortexGraphSettingsViewModel : BaseViewModel
{
    private readonly IAvatarSettingsApplicationService avatarSettingsApplicationService;

    public CortexGraphSettingsViewModel(IAvatarSettingsApplicationService avatarSettingsApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettingsApplicationService, nameof(avatarSettingsApplicationService));

        this.avatarSettingsApplicationService = avatarSettingsApplicationService;
    }

    [ObservableProperty]
    public int pollInterval;

    [ObservableProperty]
    public string dbName;

    [ObservableProperty]
    public string dbUsername;

    [ObservableProperty]
    public string dbPassword;

    [ObservableProperty]
    public string dbUrl;

    [ObservableProperty]
    public int defaultRelativeValues;

    [ObservableProperty]
    public int defaultNeuronActiveValues;

    [ObservableProperty]
    public int defaultTerminalActiveValues;

    [ObservableProperty]
    public int defaultPageSize;

    [ObservableProperty]
    public int defaultPage;

    [ObservableProperty]
    public string arangoRootPassword;

    [RelayCommand]
    private async Task GetAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();
            var cortexGraphSettings = avatarSettings.CortexGraph;

            this.PollInterval = cortexGraphSettings.PollInterval;
            this.DbName = cortexGraphSettings.DbName;
            this.DbUsername = cortexGraphSettings.DbUsername;
            this.DbPassword = cortexGraphSettings.DbPassword;
            this.DbUrl = cortexGraphSettings.DbUrl;
            this.DefaultRelativeValues = cortexGraphSettings.DefaultRelativeValues;
            this.DefaultNeuronActiveValues = cortexGraphSettings.DefaultNeuronActiveValues;
            this.DefaultTerminalActiveValues = cortexGraphSettings.DefaultTerminalActiveValues;
            this.DefaultPageSize = cortexGraphSettings.DefaultPageSize;
            this.DefaultPage = cortexGraphSettings.DefaultPage;
            this.ArangoRootPassword = cortexGraphSettings.ArangoRootPassword;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Get, Constants.Titles.CortexGraphSettings)}: {ex.Message}",
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
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.CortexGraphSettings),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            var avatarSettings = await this.avatarSettingsApplicationService.GetAsync();

            avatarSettings.CortexGraph.PollInterval = this.PollInterval;
            avatarSettings.CortexGraph.DbName = this.DbName;
            avatarSettings.CortexGraph.DbUsername = this.DbUsername;
            avatarSettings.CortexGraph.DbPassword = this.DbPassword;
            avatarSettings.CortexGraph.DbUrl = this.DbUrl;
            avatarSettings.CortexGraph.DefaultRelativeValues = this.DefaultRelativeValues;
            avatarSettings.CortexGraph.DefaultNeuronActiveValues = this.DefaultNeuronActiveValues;
            avatarSettings.CortexGraph.DefaultTerminalActiveValues = this.DefaultTerminalActiveValues;
            avatarSettings.CortexGraph.DefaultPageSize = this.DefaultPageSize;
            avatarSettings.CortexGraph.DefaultPage = this.DefaultPage;
            avatarSettings.CortexGraph.ArangoRootPassword = this.ArangoRootPassword;

            await this.avatarSettingsApplicationService.SaveAsync(avatarSettings);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
              string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.CortexGraphSettings),
              Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.CortexGraphSettings)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
