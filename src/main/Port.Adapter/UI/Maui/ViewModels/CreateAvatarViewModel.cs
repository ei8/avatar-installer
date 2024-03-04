using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using Maui.Services;
using Maui.Views;
using MetroLog.Maui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class CreateAvatarViewModel : BaseViewModel
{
    private readonly LogController logController = new();
    private readonly IAvatarApplicationService avatarApplicationService;

    public CreateAvatarViewModel(INavigationService navigationService, IAvatarApplicationService avatarApplicationService) 
        : base(navigationService)
    {
        Title = "Create Avatar";
        this.avatarApplicationService = avatarApplicationService;

        avatarApplicationService.OnCreateAvatar += () => { CreationProgress = 0.1; LoadingText = "Creating Avatar..."; };
        avatarApplicationService.OnConfiguringAvatar += () => { CreationProgress = 0.3; LoadingText = "Configuring Avatars..."; };
        avatarApplicationService.OnAvatarMapping += () => { CreationProgress = 0.5; LoadingText = "Mapping Avatar Server..."; };
        avatarApplicationService.OnAvatarSaving += () => { CreationProgress = 0.8; LoadingText = "Saving Avatar..."; };
        avatarApplicationService.OnAvatarCreated += () =>
        {
            CreationProgress = 1;
            LoadingText = "Finished!";
        };
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
    private async Task GoToHomePageAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    [RelayCommand]
    private async Task ChooseConfigurationAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            var configFile = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose a config file"
            });

            if (configFile is null)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Cancelled!",
                    $"Creating Avatar cancelled", "OK");
                return;
            }

            if (!configFile.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.CurrentPage.DisplayAlert("Invalid File!",
                    $"Configuration must be a a json file", "OK");
                return;
            }

            ConfigPath = configFile.FullPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Choosing config file", ex.ToString(), "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateAvatarAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            EditorLogs = string.Empty;

            if (string.IsNullOrEmpty(ConfigPath))
            {
                await Shell.Current.DisplayAlert("No Configuration!", "Please choose config file", "OK");
                return;
            }

            await avatarApplicationService.CreateAvatarAsync(ConfigPath);
            await Shell.Current.DisplayAlert("Success!", "Avatar Installed", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error Installing Avatar!", ex.ToString(), "OK");
        }
        finally
        {
            var logList = await logController.GetLogList();
            logList!.Reverse();
            EditorLogs = string.Join(Environment.NewLine, logList);

            IsBusy = false;
        }
    }
}
