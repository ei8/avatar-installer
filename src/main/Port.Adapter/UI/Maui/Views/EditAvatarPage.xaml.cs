using Maui.ViewModels;

namespace Maui.Views;

public partial class EditAvatarPage : ContentPage
{
	private readonly EditAvatarViewModel viewModel;

	public EditAvatarPage(EditAvatarViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}
}