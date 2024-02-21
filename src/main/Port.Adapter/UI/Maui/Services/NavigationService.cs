using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        if (parameters is null)
            await Shell.Current.GoToAsync(route);
        else
            await Shell.Current.GoToAsync(route, parameters);
    }
}
