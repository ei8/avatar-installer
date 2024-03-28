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

public partial class NeuronPermitsViewModel : EditAvatarViewModel
{
    public ObservableCollection<NeuronPermit> NeuronPermits { get; set; } = [];
    private readonly INeuronPermitApplicationService neuronPermitApplicationService;

    public NeuronPermitsViewModel(INeuronPermitApplicationService neuronPermitApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermitApplicationService, nameof(neuronPermitApplicationService));

        this.neuronPermitApplicationService = neuronPermitApplicationService;
    }

    [RelayCommand]
    private async Task GetNeuronPermitsAsync()
    {
        if (this.IsBusy)
            return;

        try
        {
            this.IsBusy = true;

            this.NeuronPermits.Clear();
            var neuronPermits = await this.neuronPermitApplicationService.GetAllAsync();

            foreach (var neuronPermit in neuronPermits)
            {
                this.NeuronPermits.Add(neuronPermit);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, $"Unable to get NeuronPermits: {ex.Message}", Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToNeuronPermitDetailsAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        await Shell.Current.GoToAsync($"{nameof(NeuronPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "NeuronPermit", neuronPermit }
            });
    }
}
