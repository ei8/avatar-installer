using CommunityToolkit.Mvvm.Input;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class EditAvatarViewModel : BaseViewModel
{
    public EditAvatarViewModel() 
    {
    }

    [RelayCommand]
    private async Task CancelEditAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}