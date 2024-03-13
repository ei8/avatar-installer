using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class UsersViewModel : EditAvatarViewModel
{
    public ObservableCollection<User> Users { get; set; } = [];
    private readonly IUserRepository UserRepository;

    public UsersViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, IUserRepository UserRepository)
        : base(editAvatarSettings, navigationService)
    {
        Title = "Neuron Permit";
        this.UserRepository = UserRepository;
    }

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
            var users = await UserRepository.GetAllAsync();

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
        }
    }

    [RelayCommand]
    private async Task GoToUserDetailsAsync(User user)
    {
        if (user is null)
            return;

        await navigationService.NavigateToAsync($"{nameof(UserDetailsPage)}",
            new Dictionary<string, object>
            {
                { "User", user }
            });
    }
}
