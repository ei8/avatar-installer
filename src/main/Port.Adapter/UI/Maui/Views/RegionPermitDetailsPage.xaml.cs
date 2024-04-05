using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

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