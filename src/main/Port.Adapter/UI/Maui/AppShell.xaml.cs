using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(CreateAvatarPage), typeof(CreateAvatarPage));
        Routing.RegisterRoute(nameof(NeuronPermitsPage), typeof(NeuronPermitsPage));
        Routing.RegisterRoute(nameof(NeuronPermitDetailsPage), typeof(NeuronPermitDetailsPage));
        Routing.RegisterRoute(nameof(RegionPermitsPage), typeof(RegionPermitsPage));
        Routing.RegisterRoute(nameof(RegionPermitDetailsPage), typeof(RegionPermitDetailsPage));
        Routing.RegisterRoute(nameof(UsersPage), typeof(UsersPage));
        Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));

        BindingContext = this;
    }

    private void ReturnToHome_Clicked(object sender, EventArgs e)
    {
        Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
