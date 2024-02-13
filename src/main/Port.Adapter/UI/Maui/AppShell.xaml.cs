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
        Routing.RegisterRoute(nameof(NeuronPermitPage), typeof(NeuronPermitPage));
    }
}
