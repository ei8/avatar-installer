using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace Maui.Views;

public partial class RegionPermitsPage : ContentPage
{
    private readonly RegionPermitsViewModel viewModel;

    public RegionPermitsPage(RegionPermitsViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        viewModel.GetRegionPermitsCommand.ExecuteAsync(this);
    }
}