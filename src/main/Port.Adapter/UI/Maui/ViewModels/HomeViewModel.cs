using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using Maui.Services;
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
    private readonly EditAvatarSettings editAvatarSettings;
    private readonly IAvatarContextService avatarContextService;
    private readonly IAvatarRepository avatarRepository;

    public HomeViewModel(IAvatarRepository avatarRepository, IAvatarContextService avatarContextService, EditAvatarSettings editAvatarSettings, INavigationService navigationService)
        : base(navigationService)
    {
        Title = "Avatar Installer";

        this.avatarContextService = avatarContextService;
        this.avatarRepository = avatarRepository;
        this.editAvatarSettings = editAvatarSettings;
    }

    [RelayCommand]
    private async Task GoToCreateAvatarAsync()
    {
        await navigationService.NavigateToAsync($"{nameof(CreateAvatarPage)}");
    }

    [RelayCommand]
    private async Task GoToEditAvatarAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
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

            editAvatarSettings.WorkingDirectory = workingDirectory.Folder.Path;
            avatarContextService.Avatar = await avatarRepository.GetByAsync(workingDirectory.Folder.Path);
            await navigationService.NavigateToAsync($"//IdentityAccessPage");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", ex.ToString(), "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}