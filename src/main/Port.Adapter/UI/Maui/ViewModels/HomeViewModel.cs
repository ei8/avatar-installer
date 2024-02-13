using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IAvatarApplicationService avatarApplicationService;

    public HomeViewModel(IAvatarApplicationService avatarApplicationService)
    {
        Title = "Avatar Installer";

        this.avatarApplicationService = avatarApplicationService;
    }

    [ObservableProperty]
    private string loadingText = string.Empty;

    [RelayCommand]
    private async Task GoToCreateAvatarAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(CreateAvatarPage)}", false);
    }

    [RelayCommand]
    private async Task GoToEditAvatarAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(EditAvatarPage)}", false);
    }

    [RelayCommand]
    private async Task CreateAvatarAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            LoadingText = "Choosing a configuration file...";

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

            LoadingText = "Creating Avatar...";

            await avatarApplicationService.CreateAvatarAsync(configFile.FullPath);
            await Shell.Current.DisplayAlert("Success!", "Avatar Installed", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error Installing Avatar!", ex.ToString(), "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditAvatarAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            LoadingText = "Choosing a directory...";
            var workingDirectory = await FolderPicker.PickAsync(default);

            if (workingDirectory.Folder is null)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Cancelled!", $"Editing avatar cancelled", "OK");
                return;
            }

            string[] requiredFiles = ["avatar.db", "d23.db", "events.db", "identity-access.db", "subscriptions.db"];

            foreach (var file in requiredFiles)
            {
                var filePath = Path.Combine(workingDirectory.Folder.Path, file);

                if (!File.Exists(filePath))
                {
                    await Shell.Current.DisplayAlert("Incomplete files!", $"{file} does not exists", "OK");
                    return;
                }
            }

            Preferences.Default.Set("WorkingDirectory", workingDirectory.Folder.Path);
            await Shell.Current.GoToAsync($"//{nameof(NeuronPermitPage)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error Editing Avatar!", "See debug log for details", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}