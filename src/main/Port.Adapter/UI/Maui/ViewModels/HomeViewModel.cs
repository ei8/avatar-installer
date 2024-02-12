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
    private readonly IAvatarApplicationService _avatarApplicationService;

    public HomeViewModel(IAvatarApplicationService avatarApplicationService)
    //public HomeViewModel()
    {
        Title = "Avatar Installer";

        _avatarApplicationService = avatarApplicationService;
    }

    [RelayCommand]
    private async Task InstallAvatarAsync()
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
                    $"Choosing a file cancelled", "OK");

                return;
            }

            if (!configFile.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.CurrentPage.DisplayAlert("Invalid File!",
                    $"Configuration must be a a json file", "OK");

                return;
            }

            await _avatarApplicationService.CreateAvatarAsync(configFile.FullPath);

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
            await Shell.Current.GoToAsync($"//{nameof(IdentityAccessPage)}");
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