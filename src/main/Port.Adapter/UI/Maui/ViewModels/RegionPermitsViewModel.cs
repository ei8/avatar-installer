using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class RegionPermitsViewModel : BaseViewModel
{
    public ObservableCollection<RegionPermitDto> RegionPermits { get; set; } = [];
    private readonly IRegionPermitRepository RegionPermitRepository;
    private readonly EditAvatarSettings editAvatarSettings;

    public RegionPermitsViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, IRegionPermitRepository RegionPermitRepository)
        : base(navigationService)
    {
        Title = "Neuron Permit";
        this.RegionPermitRepository = RegionPermitRepository;
        this.editAvatarSettings = editAvatarSettings;
    }

    [ObservableProperty]
    private bool isRefreshing;

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
            var regionPermits = await RegionPermitRepository.GetRegionPermitsAsync(workingDirectory);

            foreach (var regionPermit in regionPermits)
            {
                RegionPermits.Add(regionPermit);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get RegionPermits: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
