using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using Maui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class RegionPermitsViewModel : EditAvatarViewModel
{
    public ObservableCollection<RegionPermit> RegionPermits { get; set; } = [];
    private readonly IRegionPermitRepository RegionPermitRepository;

    public RegionPermitsViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, IRegionPermitRepository RegionPermitRepository)
        : base(editAvatarSettings, navigationService)
    {
        Title = "Neuron Permit";
        this.RegionPermitRepository = RegionPermitRepository;
    }

    [RelayCommand]
    private async Task GetRegionPermitsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            RegionPermits.Clear();
            var workingDirectory = editAvatarSettings.WorkingDirectory;
            var regionPermits = await RegionPermitRepository.GetAllAsync(workingDirectory);

            foreach (var regionPermit in regionPermits)
            {
                RegionPermits.Add(regionPermit);
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

        await navigationService.NavigateToAsync($"{nameof(RegionPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "RegionPermit", regionPermit }
            });
    }
}
