using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application.IdentityAccess;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class AddUserViewModel : EditAvatarViewModel
{
    private readonly IUserApplicationService userApplicationService;

    public AddUserViewModel(IUserApplicationService userApplicationService)
    {
        AssertionConcern.AssertArgumentNotNull(userApplicationService, nameof(userApplicationService));

        this.userApplicationService = userApplicationService;
    }

    [ObservableProperty]
    public string userId;

    [ObservableProperty]
    public string neuronId;

    [ObservableProperty]
    public string active;

    [RelayCommand]
    private async Task AddUserAsync()
    {
        if (this.IsBusy) return;

        if (string.IsNullOrEmpty(this.UserId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBeEmpty, nameof(this.UserId)),
                Constants.Prompts.Ok);
            return;
        }

        if (string.IsNullOrEmpty(this.NeuronId))
        {
            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Error,
                string.Format(Constants.Messages.CantBeEmpty, nameof(this.NeuronId)),
                Constants.Prompts.Ok);
            return;
        }

        bool isConfirmed = await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Add,
            string.Format(Constants.Messages.Confirmation, Constants.Operations.Add, Constants.Titles.User),
            Constants.Prompts.Yes, Constants.Prompts.No);

        if (!isConfirmed)
            return;

        try
        {
            this.IsBusy = true;

            bool activeParsed = int.TryParse(this.Active, out int activeValue);

            var user = new User
            {
                UserId = this.UserId,
                NeuronId = this.NeuronId,
                Active = activeParsed ? activeValue : null,
            };

            await this.userApplicationService.AddAsync(user);

            await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Success,
                string.Format(Constants.Messages.Success, Constants.Operations.Added, Constants.Titles.User),
                Constants.Prompts.Ok);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error,
                $"{string.Format(Constants.Messages.Error, Constants.Operations.Add, Constants.Titles.User)}: {ex.Message}",
                Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
