using Maui.ViewModels;

namespace Maui.Views;

public partial class IdentityAccessPage : ContentPage
{
	private readonly IdentityAccessViewModel _viewModel;

	public IdentityAccessPage(IdentityAccessViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}