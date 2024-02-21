using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class UsersViewModel : BaseViewModel
{
    public ObservableCollection<UserDto> Users { get; set; } = [];
    private readonly IUserRepository UserRepository;
    private readonly EditAvatarSettings editAvatarSettings;

    public UsersViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, IUserRepository UserRepository)
        : base(navigationService)
    {
        Title = "Neuron Permit";
        this.UserRepository = UserRepository;
        this.editAvatarSettings = editAvatarSettings;
    }

    [ObservableProperty]
    private bool isRefreshing;

    [RelayCommand]
    private async Task GetUsersAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            Users.Clear();
            var workingDirectory = editAvatarSettings.WorkingDirectory;
            var users = await UserRepository.GetUsersAsync(workingDirectory);

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get Users: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
