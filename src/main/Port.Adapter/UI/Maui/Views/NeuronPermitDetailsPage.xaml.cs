using Maui.ViewModels;

namespace Maui.Views;

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