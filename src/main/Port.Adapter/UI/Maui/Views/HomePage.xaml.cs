using Maui.ViewModels;

namespace Maui.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel _viewModel;

	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"//{nameof(IdentityAccessPage)}");
    }
}