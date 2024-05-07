using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views.AvatarSettings;

public partial class CortexLibrarySettingsPage : ContentPage
{
    private readonly CortexLibrarySettingsViewModel viewModel;

    public CortexLibrarySettingsPage(CortexLibrarySettingsViewModel viewModel)
	{
		InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        this.viewModel.GetCommand.ExecuteAsync(this);
    }
}