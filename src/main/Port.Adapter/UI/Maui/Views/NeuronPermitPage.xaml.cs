using Maui.ViewModels;

namespace Maui.Views;

public partial class NeuronPermitPage : ContentPage
{
	private readonly NeuronPermitViewModel viewModel;

	public NeuronPermitPage(NeuronPermitViewModel viewModel)
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