using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace Maui.Views;

public partial class CreateAvatarPage : ContentPage
{
	private readonly CreateAvatarViewModel viewModel;

	public CreateAvatarPage(CreateAvatarViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}
}