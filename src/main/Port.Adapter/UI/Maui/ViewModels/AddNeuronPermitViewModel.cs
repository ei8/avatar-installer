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

public partial class AddNeuronPermitViewModel : EditAvatarViewModel
{
    private readonly INeuronPermitApplicationService neuronPermitApplicationService;

    public AddNeuronPermitViewModel(INeuronPermitApplicationService neuronPermitApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermitApplicationService, nameof(neuronPermitApplicationService));

        this.neuronPermitApplicationService = neuronPermitApplicationService;
    }

    [ObservableProperty]
    public string userNeuronId;

    [ObservableProperty]
    public string neuronId;

    [ObservableProperty]
    public string expirationDate;

    [RelayCommand]
    private async Task AddNeuronPermitAsync()
    {
        if (this.IsBusy) return;

        if (string.IsNullOrEmpty(this.UserNeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBeEmpty, nameof(UserNeuronId)),
                Constants.Prompts.Ok);
            return;
        }

        if (string.IsNullOrEmpty(this.NeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBeEmpty, nameof(NeuronId)),
                Constants.Prompts.Ok);
            return;
        }

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Add,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Add, Constants.Titles.NeuronPermit),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            var neuronPermit = new NeuronPermit
            {
                UserNeuronId = this.UserNeuronId,
                NeuronId = this.NeuronId,
                ExpirationDate = !string.IsNullOrEmpty(this.ExpirationDate) ? this.ExpirationDate : null,
            };

            await this.neuronPermitApplicationService.AddAsync(neuronPermit);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Added, Constants.Titles.NeuronPermit),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Add, Constants.Titles.NeuronPermit)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
