using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace Maui.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel viewModel;

	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}