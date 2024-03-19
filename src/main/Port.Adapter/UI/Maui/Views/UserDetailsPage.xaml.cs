using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

public partial class UserDetailsPage : ContentPage
{
    private readonly UserDetailsViewModel viewModel;
    public UserDetailsPage(UserDetailsViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }
}