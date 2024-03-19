using ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

namespace Maui.Views;

public partial class NeuronPermitsPage : ContentPage
{
    private readonly NeuronPermitsViewModel viewModel;

    public NeuronPermitsPage(NeuronPermitsViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        viewModel.GetNeuronPermitsCommand.ExecuteAsync(this);
    }
}