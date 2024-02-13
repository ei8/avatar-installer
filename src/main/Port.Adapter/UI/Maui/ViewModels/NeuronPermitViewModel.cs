using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.External;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class NeuronPermitViewModel : BaseViewModel
{
    public ObservableCollection<NeuronPermitDto> NeuronPermits { get; set; } = [];
    private readonly INeuronPermitRepository neuronPermitRepository;

    public NeuronPermitViewModel(INeuronPermitRepository neuronPermitRepository)
    {
        Title = "Neuron Permit";
        this.neuronPermitRepository = neuronPermitRepository;
    }

    [ObservableProperty]
    private bool isRefreshing;

    [RelayCommand]
    private async Task GetNeuronPermitsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            NeuronPermits.Clear();
            var workingDirectory = Preferences.Default.Get("WorkingDirectory", string.Empty);
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
            IsRefreshing = false;
        }
    }
}
