using Maui.ViewModels;

namespace Maui.Views;

public partial class IdentityAccessPage : ContentPage
{
	private readonly IdentityAccessViewModel viewModel;

	public IdentityAccessPage(IdentityAccessViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}
}