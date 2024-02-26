using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Domain.Model.DTO;
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

    public NeuronPermitDetailsViewModel(EditAvatarSettings editAvatarSettings, INavigationService navigationService, INeuronPermitRepository neuronPermitRepository)
        : base(editAvatarSettings, navigationService)
    {
        this.neuronPermitRepository = neuronPermitRepository;
    }

    [ObservableProperty]
    private NeuronPermit? neuronPermit;

    [RelayCommand]
    private async Task UpdateNeuronPermitAsync()
    {
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update Neuron Permit", "Are you sure you want to update this Neuron Permit",
                            "Yes", "No");

        if (!isConfirmed)
            return;

        try
        {
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

