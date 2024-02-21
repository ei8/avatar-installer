using Maui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModels;

public partial class IdentityAccessViewModel : BaseViewModel
{
    public IdentityAccessViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
        Title = "Identity Access";
    }
}
