using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels.AvatarSettings;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views.AvatarSettings;

public partial class CortexChatNucleusSettingsPage : ContentPage
{
    private readonly CortexChatNucleusSettingsViewModel viewModel;

    public CortexChatNucleusSettingsPage(CortexChatNucleusSettingsViewModel viewModel)
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