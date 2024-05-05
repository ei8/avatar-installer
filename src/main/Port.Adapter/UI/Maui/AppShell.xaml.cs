using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views.AvatarSettings;

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

        Routing.RegisterRoute(nameof(EventSourcingSettingsPage), typeof(EventSourcingSettingsPage));
        Routing.RegisterRoute(nameof(CortexGraphSettingsPage), typeof(CortexGraphSettingsPage));
        Routing.RegisterRoute(nameof(AvatarApiSettingsPage), typeof(AvatarApiSettingsPage));
        Routing.RegisterRoute(nameof(IdentityAccessSettingsPage), typeof(IdentityAccessSettingsPage));
        Routing.RegisterRoute(nameof(CortexLibrarySettingsPage), typeof(CortexLibrarySettingsPage));
        Routing.RegisterRoute(nameof(CortexDiaryNucleusSettingsPage), typeof(CortexDiaryNucleusSettingsPage));

        BindingContext = this;
    }

    private void ReturnToHome_Clicked(object sender, EventArgs e)
    {
        Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
