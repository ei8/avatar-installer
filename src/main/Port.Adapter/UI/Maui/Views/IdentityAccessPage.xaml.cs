using Maui.ViewModels;

namespace Maui.Views;

public partial class IdentityAccessPage : TabbedPage
{
	private readonly IdentityAccessViewModel viewModel;
	private readonly NeuronPermitsViewModel neuronPermitsViewModel;

	public IdentityAccessPage(IdentityAccessViewModel viewModel, NeuronPermitsViewModel neuronPermitsViewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		this.neuronPermitsViewModel = neuronPermitsViewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		neuronPermitsViewModel.GetNeuronPermitsCommand.ExecuteAsync(null);
    }
}