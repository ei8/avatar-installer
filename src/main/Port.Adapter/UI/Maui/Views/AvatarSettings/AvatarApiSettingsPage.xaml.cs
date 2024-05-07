using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views.AvatarSettings;

public partial class AvatarApiSettingsPage : ContentPage
{
    private readonly AvatarApiSettingsViewModel viewModel;

    public AvatarApiSettingsPage(AvatarApiSettingsViewModel viewModel)
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