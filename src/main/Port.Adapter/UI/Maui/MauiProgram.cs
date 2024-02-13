using CommunityToolkit.Maui;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.External;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using ei8.Avatar.Installer.IO.Process.Services.Avatars;
using ei8.Avatar.Installer.IO.Process.Services.External;
using ei8.Avatar.Installer.IO.Process.Services.Settings;
using ei8.Avatar.Installer.IO.Process.Services.Template;
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

        builder.Services.AddLogging(
            configure =>
            {
                configure.AddDebug();
            }
        );
#endif

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<IdentityAccessPage>();
        builder.Services.AddSingleton<IdentityAccessViewModel>();

        builder.Services.AddSingleton<NeuronPermitPage>();
        builder.Services.AddSingleton<NeuronPermitViewModel>();

        builder.Services.AddScoped<ISettingsService, SettingsService>()
                .AddScoped<ITemplateService, GithubTemplateService>()
                .AddScoped<IConfigurationRepository, JsonConfigurationRepository>()
                .AddScoped<IAvatarRepository, AvatarRepository>()
                .AddScoped<IAvatarMapperService, AvatarMapperService>()
                .AddScoped<IAvatarServerRepository, AvatarServerRepository>()
                .AddScoped<IAvatarServerMapperService, AvatarServerMapperService>()
                .AddScoped<IAvatarApplicationService, AvatarApplicationService>();

        builder.Services.AddAutoMapper(typeof(AvatarAutoMapperProfile));

        builder.Services.AddSingleton<INeuronPermitRepository, NeuronPermitRepository>();

        return builder.Build();
    }
}
