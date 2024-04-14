using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class AddRegionPermitViewModel : EditAvatarViewModel
{
    private readonly IRegionPermitApplicationService regionPermitApplicationService;

    public AddRegionPermitViewModel(IRegionPermitApplicationService regionPermitApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermitApplicationService, nameof(regionPermitApplicationService));

        this.regionPermitApplicationService = regionPermitApplicationService;
    }

    [ObservableProperty]
    public string userNeuronId;

    [ObservableProperty]
    public string regionNeuronId;

    [ObservableProperty]
    public string writeLevel;

    [ObservableProperty]
    public string readLevel;

    [RelayCommand]
    private async Task AddRegionPermitAsync()
    {
        if (this.IsBusy) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Add,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Add, Constants.Titles.RegionPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            bool writeLevelParsed = int.TryParse(this.WriteLevel, out int writeLevelValue);
            bool readLevelParsed = int.TryParse(this.ReadLevel, out int readLevelValue);

            var regionPermit = new RegionPermit
            {
                UserNeuronId = !string.IsNullOrEmpty(this.UserNeuronId) ? this.UserNeuronId : null,
                RegionNeuronId = !string.IsNullOrEmpty(this.RegionNeuronId) ? this.RegionNeuronId : null,
                WriteLevel = readLevelParsed ? writeLevelValue : null,
                ReadLevel = readLevelParsed ? readLevelValue : null
            };

            await this.regionPermitApplicationService.AddAsync(regionPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Added, Constants.Titles.RegionPermit),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Add, Constants.Titles.RegionPermit)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
