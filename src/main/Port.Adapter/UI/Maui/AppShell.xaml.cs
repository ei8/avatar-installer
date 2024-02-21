using CommunityToolkit.Maui.Storage;
using Maui.Views;

namespace Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(CreateAvatarPage), typeof(CreateAvatarPage));
        Routing.RegisterRoute(nameof(EditAvatarPage), typeof(EditAvatarPage));
        Routing.RegisterRoute(nameof(IdentityAccessPage), typeof(IdentityAccessPage));
        Routing.RegisterRoute(nameof(NeuronPermitsPage), typeof(NeuronPermitsPage));
        Routing.RegisterRoute(nameof(RegionPermitsPage), typeof(RegionPermitsPage));
        Routing.RegisterRoute(nameof(UsersPage), typeof(UsersPage));
        Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
    }
}
