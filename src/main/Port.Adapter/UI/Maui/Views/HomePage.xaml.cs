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
}