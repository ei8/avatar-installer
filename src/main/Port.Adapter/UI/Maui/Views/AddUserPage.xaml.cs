using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

public partial class AddUserPage : ContentPage
{
    private readonly AddUserViewModel viewModel;

    public AddUserPage(AddUserViewModel viewModel)
	{
		InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }
}