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

        try
        {
            await Shell.Current.DisplayAlert("Success!", "Avatar Installed", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error Installing Avatar!", "See debug log for details", "OK");
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