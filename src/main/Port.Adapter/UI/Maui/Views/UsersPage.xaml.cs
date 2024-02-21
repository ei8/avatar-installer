using Maui.ViewModels;

namespace Maui.Views;

public partial class UsersPage : ContentPage
{
    private readonly UsersViewModel viewModel;

    public UsersPage(UsersViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        viewModel.GetUsersCommand.ExecuteAsync(this);
    }
}