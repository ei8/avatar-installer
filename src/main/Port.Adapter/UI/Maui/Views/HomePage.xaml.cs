using Maui.ViewModels;

namespace Maui.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel viewModel;

	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = viewModel;
	}
}