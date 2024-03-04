using CommunityToolkit.Mvvm.Input;
using Maui.Services;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class EditAvatarViewModel : BaseViewModel
{
    protected readonly EditAvatarSettings editAvatarSettings;

    public EditAvatarViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService) : base(navigationService)
    {
        this.editAvatarSettings = editAvatarSettings;
    }

    [RelayCommand]
    private async Task CancelEditAsync()
    {
        await navigationService.NavigateToAsync("..");
    }
}