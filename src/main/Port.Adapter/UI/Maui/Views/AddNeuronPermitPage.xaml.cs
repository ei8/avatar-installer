using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

public partial class AddNeuronPermitPage : ContentPage
{
    private readonly AddNeuronPermitViewModel viewModel;

    public AddNeuronPermitPage(AddNeuronPermitViewModel viewModel)
	{
		InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }
}