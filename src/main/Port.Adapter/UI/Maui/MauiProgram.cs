using CommunityToolkit.Maui;
using Maui.ViewModels;
using Maui.Views;
using Microsoft.Extensions.Logging;

namespace Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<IdentityAccessPage>();
        builder.Services.AddSingleton<IdentityAccessViewModel>();

        return builder.Build();
    }
}
