using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;
using Maui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

[QueryProperty("User", "User")]
public partial class UserDetailsViewModel : EditAvatarViewModel
{
    private readonly IUserRepository userRepository;

    public UserDetailsViewModel(INavigationService navigationService, IUserRepository userRepository)
        : base(navigationService)
    {
        this.userRepository = userRepository;
    }

    [ObservableProperty]
    private User? user;

    [RelayCommand]
    private async Task UpdateUserAsync()
    {
        if (User is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update User", "Are you sure you want to update this User?",
                            "Yes", "No");

        if (!isConfirmed)
            return;

        try
        {
            await userRepository.UpdateAsync(User!);

            await Shell.Current.CurrentPage.DisplayAlert("Success!",
                $"User updated", "OK");

            await navigationService.NavigateToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert("Error!",
                $"Unable to update User: {ex.Message}", "OK");
        }
    }
}
