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

[QueryProperty(nameof(NeuronPermit), "NeuronPermit")]
[QueryProperty(nameof(Mode), "Mode")]
public partial class NeuronPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly INeuronPermitApplicationService neuronPermitApplicationService;

    public NeuronPermitDetailsViewModel(INeuronPermitApplicationService neuronPermitApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermitApplicationService, nameof(neuronPermitApplicationService));

        this.neuronPermitApplicationService = neuronPermitApplicationService;
    }

    [ObservableProperty]
    private NeuronPermit neuronPermit;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditing))]
    private Mode mode;

    public bool IsEditing => this.Mode == Mode.Edit;

    [RelayCommand]
    private async Task SaveNeuronPermitAsync()
    {
        if (this.NeuronPermit is null || this.IsBusy) return;

        if (string.IsNullOrEmpty(this.NeuronPermit.UserNeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBe, nameof(this.NeuronPermit.UserNeuronId), Constants.Operations.Empty),
                Constants.Prompts.Ok);
            return;
        }

        if (string.IsNullOrEmpty(this.NeuronPermit.NeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBe, nameof(this.NeuronPermit.NeuronId), Constants.Operations.Empty),
                Constants.Prompts.Ok);
            return;
        }

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Remove,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Remove, Constants.Titles.NeuronPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            if (this.Mode == Mode.Create)
            {
                var exists = await this.neuronPermitApplicationService.CheckIfExistsAsync(
                    this.NeuronPermit.UserNeuronId, this.NeuronPermit.NeuronId);

                if (exists)
                {
                    await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                        string.Format(Constants.Messages.AlreadyExists, Constants.Titles.NeuronPermit),
                        Constants.Prompts.Ok);
                    return;
                }
            }
            await this.neuronPermitApplicationService.SaveAsync(this.NeuronPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Saved, Constants.Titles.NeuronPermit),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Save, Constants.Titles.NeuronPermit)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveNeuronPermitAsync()
    {
        if (this.NeuronPermit is null || this.IsBusy) return;

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Save,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Remove, Constants.Titles.NeuronPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;
            
            await this.neuronPermitApplicationService.RemoveAsync(this.NeuronPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Removed, Constants.Titles.NeuronPermit),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Remove, Constants.Titles.NeuronPermit)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}

