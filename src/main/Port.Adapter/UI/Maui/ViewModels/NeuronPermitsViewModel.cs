using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.DTO;
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

public partial class NeuronPermitsViewModel : EditAvatarViewModel
{
    public ObservableCollection<NeuronPermit> NeuronPermits { get; set; } = [];
    private readonly INeuronPermitRepository neuronPermitRepository;

    public NeuronPermitsViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, INeuronPermitRepository neuronPermitRepository)
        : base(editAvatarSettings, navigationService)
    {
        Title = "Neuron Permit";
        this.neuronPermitRepository = neuronPermitRepository;
    }

    [RelayCommand]
    private async Task GetNeuronPermitsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            NeuronPermits.Clear();
            var workingDirectory = editAvatarSettings.WorkingDirectory;
            var neuronPermits = await neuronPermitRepository.GetNeuronPermitsAsync(workingDirectory);

            foreach (var neuronPermit in neuronPermits)
            {
                NeuronPermits.Add(neuronPermit);
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

        await navigationService.NavigateToAsync($"{nameof(NeuronPermitDetailsPage)}",
            new Dictionary<string, object>
            {
                { "NeuronPermit", neuronPermit }
            });
    }
}
