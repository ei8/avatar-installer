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
    public EditAvatarViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    [RelayCommand]
    private async Task CancelEditAsync()
    {
        await navigationService.NavigateToAsync("..");
    }
}