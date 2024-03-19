using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class RegionPermitsViewModel : EditAvatarViewModel
{
    public ObservableCollection<RegionPermit> RegionPermits { get; set; } = [];
    private readonly IRegionPermitApplicationService regionPermitApplicationService;

    public RegionPermitsViewModel(IRegionPermitApplicationService regionPermitApplicationService)
    {
        Title = "Neuron Permit";
        this.regionPermitApplicationService = regionPermitApplicationService;
    }

    [RelayCommand]
    private async Task GetRegionPermitsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            this.RegionPermits.Clear();
            var regionPermits = await regionPermitApplicationService.GetAllAsync();

            foreach (var regionPermit in regionPermits)
            {
                this.RegionPermits.Add(regionPermit);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get Region Permits: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegionPermitDetailsAsync(RegionPermit regionPermit)
    {
        if (regionPermit is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(RegionPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "RegionPermit", regionPermit }
            });
    }
}
