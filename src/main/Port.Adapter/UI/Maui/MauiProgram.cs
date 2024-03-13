using CommunityToolkit.Maui;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using ei8.Avatar.Installer.IO.Process.Services.Avatars;
using ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;
using ei8.Avatar.Installer.IO.Process.Services.Settings;
using ei8.Avatar.Installer.IO.Process.Services.Template;
using Maui.Services;
using Maui.ViewModels;
using Maui.Views;
using MetroLog.MicrosoftExtensions;
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

        builder.Logging.AddInMemoryLogger(
                options =>
                {
                    options.MaxLines = 1024;
                    options.MinLevel = LogLevel.Debug;
                    options.MaxLevel = LogLevel.Critical;
                });

        #region Maui Pages
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<CreateAvatarPage>();
        builder.Services.AddTransient<NeuronPermitsPage>();
        builder.Services.AddTransient<NeuronPermitDetailsPage>();
        builder.Services.AddTransient<RegionPermitsPage>();
        builder.Services.AddTransient<RegionPermitDetailsPage>();
        builder.Services.AddTransient<UsersPage>();
        builder.Services.AddTransient<UserDetailsPage>();
        #endregion

        #region Maui ViewModels
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<CreateAvatarViewModel>();
        builder.Services.AddTransient<NeuronPermitsViewModel>();
        builder.Services.AddTransient<NeuronPermitDetailsViewModel>();
        builder.Services.AddTransient<RegionPermitsViewModel>();
        builder.Services.AddTransient<RegionPermitDetailsViewModel>();
        builder.Services.AddTransient<UsersViewModel>();
        builder.Services.AddTransient<UserDetailsViewModel>();
        #endregion

        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSingleton<EditAvatarSettings>();

        builder.Services.AddScoped<ISettingsService, SettingsService>()
                .AddScoped<ITemplateService, GithubTemplateService>()
                .AddScoped<IConfigurationRepository, JsonConfigurationRepository>()
                .AddScoped<IAvatarRepository, AvatarRepository>()
                .AddScoped<IAvatarMapperService, AvatarMapperService>()
                .AddScoped<IAvatarServerRepository, AvatarServerRepository>()
                .AddScoped<IAvatarServerMapperService, AvatarServerMapperService>()
                .AddScoped<IAvatarApplicationService, AvatarApplicationService>();
        builder.Services.AddAutoMapper(typeof(AvatarAutoMapperProfile));


        //builder.Services.AddSingleton<IAvatarContextService, class>();

        builder.Services.AddSingleton<INeuronPermitRepository, NeuronPermitRepository>();
        builder.Services.AddSingleton<IRegionPermitRepository, RegionPermitRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        return builder.Build();
    }
}
