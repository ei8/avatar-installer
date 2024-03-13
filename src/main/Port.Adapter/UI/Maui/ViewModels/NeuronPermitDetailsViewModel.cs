using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Maui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

[QueryProperty("NeuronPermit", "NeuronPermit")]
public partial class NeuronPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly INeuronPermitRepository neuronPermitRepository;

    public NeuronPermitDetailsViewModel(INavigationService navigationService, INeuronPermitRepository neuronPermitRepository)
        : base(navigationService)
    {
        this.neuronPermitRepository = neuronPermitRepository;
    }

    [ObservableProperty]
    private NeuronPermit? neuronPermit;

    [RelayCommand]
    private async Task UpdateNeuronPermitAsync()
    {
        if (NeuronPermit is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update Neuron Permit", "Are you sure you want to update this Neuron Permit",
                            "Yes", "No");

        if (!isConfirmed)
            return;

        try
        {
            await neuronPermitRepository.UpdateAsync(NeuronPermit!);

            await Shell.Current.CurrentPage.DisplayAlert("Success!",
                $"Neuron Permit updated", "OK");

            await navigationService.NavigateToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert("Error!",
                $"Unable to update Neuron Permit: {ex.Message}", "OK");
        }
    }
}

