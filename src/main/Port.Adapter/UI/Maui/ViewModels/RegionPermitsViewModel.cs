using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using neurUL.Common.Domain.Model;
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
        AssertionConcern.AssertArgumentNotNull(regionPermitApplicationService, nameof(regionPermitApplicationService));

        this.regionPermitApplicationService = regionPermitApplicationService;
    }

    [RelayCommand]
    private async Task GetRegionPermitsAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

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
            await Shell.Current.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Get, Constants.Titles.RegionPermit)}s: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditRegionPermitAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        await Shell.Current.GoToAsync($"{nameof(RegionPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "RegionPermit", regionPermit },
                { "Mode", Mode.Edit },
            });
    }

    [RelayCommand]
    private async Task CreateRegionPermitAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(RegionPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "RegionPermit", new RegionPermit() },
                { "Mode", Mode.Create },
            });
    }
}
