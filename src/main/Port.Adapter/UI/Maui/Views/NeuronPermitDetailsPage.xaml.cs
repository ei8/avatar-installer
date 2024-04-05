using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

public partial class NeuronPermitDetailsPage : ContentPage
{
	private readonly NeuronPermitDetailsViewModel viewModel;
	public NeuronPermitDetailsPage(NeuronPermitDetailsViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}
}