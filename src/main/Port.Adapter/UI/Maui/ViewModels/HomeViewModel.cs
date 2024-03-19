using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IAvatarContextService avatarContextService;
    private readonly IAvatarRepository avatarRepository;

    public HomeViewModel(IAvatarRepository avatarRepository, IAvatarContextService avatarContextService)
    {
        Title = "Avatar Installer";

        this.avatarContextService = avatarContextService;
        this.avatarRepository = avatarRepository;
    }

    [RelayCommand]
    private async Task GoToCreateAvatarAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(CreateAvatarPage)}");
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

            avatarContextService.Avatar = await avatarRepository.GetByAsync(workingDirectory.Folder.Path);
            await Shell.Current.GoToAsync($"//IdentityAccessPage");
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