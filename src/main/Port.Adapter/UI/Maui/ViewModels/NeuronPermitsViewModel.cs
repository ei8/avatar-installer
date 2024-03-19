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

//namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;
namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class NeuronPermitsViewModel : EditAvatarViewModel
{
    public ObservableCollection<NeuronPermit> NeuronPermits { get; set; } = [];
    private readonly INeuronPermitApplicationService neuronPermitApplicationService;

    public NeuronPermitsViewModel(INeuronPermitApplicationService neuronPermitApplicationService)
    {
        Title = "Neuron Permit";
        this.neuronPermitApplicationService = neuronPermitApplicationService;
    }

    [RelayCommand]
    private async Task GetNeuronPermitsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

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
            await Shell.Current.DisplayAlert("Error!", $"Unable to get NeuronPermits: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToNeuronPermitDetailsAsync(NeuronPermit neuronPermit)
    {
        if (neuronPermit is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(NeuronPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "NeuronPermit", neuronPermit }
            });
    }
}
