using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using neurUL.Common.Domain.Model;
using System.Diagnostics;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IAvatarContextService avatarContextService;
    private readonly IAvatarItemReadRepository avatarItemReadRepository;

    public HomeViewModel(IAvatarItemReadRepository avatarItemReadRepository, IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarItemReadRepository, nameof(avatarItemReadRepository));
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
        this.avatarItemReadRepository = avatarItemReadRepository;
    }

    [RelayCommand]
    private async Task GoToCreateAvatarAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(CreateAvatarPage)}");
    }

    [RelayCommand]
    private async Task GoToEditAvatarAsync()
    {
        if (this.IsBusy)
            return;

        this.IsBusy = true;

        try
        {
            var workingDirectory = await FolderPicker.PickAsync(default);

            if (workingDirectory.Folder is null)
            {
                await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Cancelled,
                    Constants.Messages.EditingCancelled, Constants.Prompts.Ok);
                return;
            }

            string[] requiredFiles = [
                Constants.Databases.AvatarDb, 
                Constants.Databases.Un8yDb,
                Constants.Databases.EventsDb,
                Constants.Databases.Iden8yDb, 
                Constants.Databases.SubscriptionsDb
            ];

            foreach (var file in requiredFiles)
            {
                var filePath = Path.Combine(workingDirectory.Folder.Path, file);

                if (!File.Exists(filePath))
                {
                    await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, $"{file} does not exists", Constants.Prompts.Ok);
                    return;
                }
            }

            this.avatarContextService.Avatar = await this.avatarItemReadRepository.GetByAsync(workingDirectory.Folder.Path);
            await Shell.Current.GoToAsync($"//IdentityAccessPage");
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
}