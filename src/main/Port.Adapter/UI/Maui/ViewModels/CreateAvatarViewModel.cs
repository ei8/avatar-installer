using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using MetroLog.Maui;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class CreateAvatarViewModel : BaseViewModel
{
    private readonly LogController logController = new();
    private readonly IAvatarApplicationService avatarApplicationService;
    private readonly IProgressService progressService;
    private AvatarServerConfiguration AvatarServerConfiguration = new(string.Empty);

    public CreateAvatarViewModel(IAvatarApplicationService avatarApplicationService, IProgressService progressService) 
    {
        AssertionConcern.AssertArgumentNotNull(avatarApplicationService, nameof(avatarApplicationService));
        AssertionConcern.AssertArgumentNotNull(progressService, nameof(progressService));

        this.avatarApplicationService = avatarApplicationService;
        this.progressService = progressService;

        this.progressService.ProgressChanged += ProgressService_ProgressChanged;
        this.progressService.DescriptionChanged += ProgressService_DescriptionChanged;
    }

    private void ProgressService_DescriptionChanged(object sender, EventArgs e)
    {
        this.LoadingText = this.progressService.Description;
    }

    private void ProgressService_ProgressChanged(object sender, EventArgs e)
    {
        this.CreationProgress = this.progressService.Progress;
    }

    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private string ownerName = string.Empty;
    [ObservableProperty]
    private string ownerUserId = string.Empty;

    partial void OnNameChanged(string value)
    {
        if (AvatarServerConfiguration?.Avatars?.Count() > 0)
        {
            AvatarServerConfiguration.Avatars[0].Name = value;
        }
    }

    partial void OnOwnerNameChanged(string value)
    {
        if (AvatarServerConfiguration?.Avatars?.Count() > 0)
        {
            AvatarServerConfiguration.Avatars[0].OwnerName = value;
        }
    }

    partial void OnOwnerUserIdChanged(string value)
    {
        if (AvatarServerConfiguration?.Avatars?.Count() > 0)
        {
            AvatarServerConfiguration.Avatars[0].OwnerUserId = value;
        }
    }
    [ObservableProperty]
    private string configPath = string.Empty;

    [ObservableProperty]
    private string editorLogs = string.Empty;

    [ObservableProperty]
    private double creationProgress = 0.0;

    [ObservableProperty]
    private string loadingText = "Haven't started...";

    [RelayCommand]
    private async Task ChooseConfigurationAsync()
    {
        if (IsBusy)
            return;

        this.IsBusy = true;

        try
        {
            var configFile = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = Constants.Messages.ChooseConfig
            });

            if (configFile is null)
            {
                await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Cancelled,
                    Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
                return;
            }

            if (!configFile.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Invalid,
                    Constants.Messages.InvalidConfig, Constants.Prompts.Ok);
                return;
            }

            ConfigPath = configFile.FullPath;
            this.AvatarServerConfiguration = await avatarApplicationService.ReadAvatarConfiguration(ConfigPath);
            this.Name = this.AvatarServerConfiguration.Avatars[0].Name;
            this.OwnerName = this.AvatarServerConfiguration.Avatars[0].OwnerName;
            this.OwnerUserId = this.AvatarServerConfiguration.Avatars[0].OwnerUserId;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateAvatarAsync()
    {
        if (this.IsBusy)
            return;

        this.IsBusy = true;

        try
        {
            EditorLogs = string.Empty;

            if (string.IsNullOrEmpty(ConfigPath))
            {
                await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
                return;
            }
            await avatarApplicationService.CreateAvatarAsync(AvatarServerConfiguration);
            await Shell.Current.DisplayAlert(Constants.Statuses.Success, Constants.Messages.AvatarInstalled, Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        finally
        {
            var logList = await logController.GetLogList();
            logList!.Reverse();
            EditorLogs = string.Join(Environment.NewLine, logList);

            this.IsBusy = false;
        }
    }

}
