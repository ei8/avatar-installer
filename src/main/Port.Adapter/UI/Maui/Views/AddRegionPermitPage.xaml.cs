using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

public partial class AddRegionPermitPage : ContentPage
{
    private readonly AddRegionPermitViewModel viewModel;

    public AddRegionPermitPage(AddRegionPermitViewModel viewModel)
	{
		InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }
}