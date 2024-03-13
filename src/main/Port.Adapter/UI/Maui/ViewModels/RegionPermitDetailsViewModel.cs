using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

[QueryProperty("RegionPermit", "RegionPermit")]
public partial class RegionPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly IRegionPermitRepository regionPermitRepository;

    public RegionPermitDetailsViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, IRegionPermitRepository regionPermitRepository)
        : base(editAvatarSettings, navigationService)
    {
        this.regionPermitRepository = regionPermitRepository;
    }

    [ObservableProperty]
    private RegionPermit? regionPermit;

    [RelayCommand]
    private async Task UpdateRegionPermitAsync()
    {
        if (RegionPermit is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update Region Permit", "Are you sure you want to update this Region Permit",
                            "Yes", "No");

        if (!isConfirmed)
            return;

        try
        {
            var workingDirectory = editAvatarSettings.WorkingDirectory;
            await regionPermitRepository.UpdateAsync(workingDirectory, RegionPermit!);

            await Shell.Current.CurrentPage.DisplayAlert("Success!",
                $"Region Permit updated", "OK");

            await navigationService.NavigateToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert("Error!",
                $"Unable to update Region Permit: {ex.Message}", "OK");
        }
    }
}
