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

[QueryProperty(nameof(RegionPermit), "RegionPermit")]
[QueryProperty(nameof(Mode), "Mode")]
public partial class RegionPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly IRegionPermitApplicationService regionPermitApplicationService;

    public RegionPermitDetailsViewModel(IRegionPermitApplicationService regionPermitApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermitApplicationService, nameof(regionPermitApplicationService));

        this.regionPermitApplicationService = regionPermitApplicationService;
    }

    [ObservableProperty]
    private RegionPermit regionPermit;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditing))]
    private Mode mode;

    public bool IsEditing => this.Mode == Mode.Edit;

    [RelayCommand]
    private async Task SaveRegionPermitAsync()
    {
        if (this.RegionPermit is null || this.IsBusy) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Save, 
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Save, Constants.Titles.RegionPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            await this.regionPermitApplicationService.SaveAsync(this.RegionPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.RegionPermit), 
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.RegionPermit)}: {ex.Message}", 
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveRegionPermitAsync()
    {
        if (this.RegionPermit is null || this.IsBusy) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Remove,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Remove, Constants.Titles.RegionPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            await this.regionPermitApplicationService.RemoveAsync(RegionPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Removed, Constants.Titles.RegionPermit),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Remove, Constants.Titles.RegionPermit)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
