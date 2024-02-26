using Maui.ViewModels;

namespace Maui.Views;

public partial class RegionPermitDetailsPage : ContentPage
{
	private readonly RegionPermitDetailsViewModel viewModel;
	public RegionPermitDetailsPage(RegionPermitDetailsViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}
}