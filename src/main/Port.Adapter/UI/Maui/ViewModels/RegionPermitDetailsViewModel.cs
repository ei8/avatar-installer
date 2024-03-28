using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

[QueryProperty("RegionPermit", "RegionPermit")]
public partial class RegionPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly IRegionPermitApplicationService regionPermitApplicationService;

    public RegionPermitDetailsViewModel(IRegionPermitApplicationService regionPermitApplicationService)
    {
        this.regionPermitApplicationService = regionPermitApplicationService;
    }

    [ObservableProperty]
    private RegionPermit? regionPermit;

    [RelayCommand]
    private async Task UpdateRegionPermitAsync()
    {
        if (this.RegionPermit is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update Region Permit", "Are you sure you want to update this Region Permit",
                            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            await this.regionPermitApplicationService.UpdateAsync(RegionPermit!);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                "Region Permit updated", Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                "Unable to update Region Permit: {ex.Message}", Constants.Prompts.Ok);
        }
    }
}
