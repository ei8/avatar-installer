using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

[QueryProperty("NeuronPermit", "NeuronPermit")]
public partial class NeuronPermitDetailsViewModel : EditAvatarViewModel
{
    private readonly INeuronPermitApplicationService neuronPermitApplicationService;

    public NeuronPermitDetailsViewModel(INeuronPermitApplicationService neuronPermitApplicationService)
    {
        this.neuronPermitApplicationService = neuronPermitApplicationService;
    }

    [ObservableProperty]
    private NeuronPermit? neuronPermit;

    [RelayCommand]
    private async Task UpdateNeuronPermitAsync()
    {
        if (this.NeuronPermit is null) return;
        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Update Neuron Permit", "Are you sure you want to update this Neuron Permit",
                            "Yes", "No");

        if (!isConfirmed)
            return;

        try
        {
            await this.neuronPermitApplicationService.UpdateAsync(NeuronPermit!);

            await Shell.Current.CurrentPage.DisplayAlert("Success!",
                $"Neuron Permit updated", "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            await Shell.Current.CurrentPage.DisplayAlert("Error!",
                $"Unable to update Neuron Permit: {ex.Message}", "OK");
        }
    }
}

