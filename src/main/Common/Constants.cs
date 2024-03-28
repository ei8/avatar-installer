namespace ei8.Avatar.Installer.Common
{
    public sealed class Constants
    {
        public sealed class Filenames
        {
            public const string TraefikToml = "traefik.toml";
            public const string SshConfig = "ssh_config";

            public const string StartTraefikBat = "start - traefik.bat";
            public const string StartEi8SiteBat = "start - ei8.site.bat";
            public const string LoopStartEi8SiteBat = "loop start - ei8.site.bat";
            public const string AutoLockCmd = "autolock.cmd";
        }

        public sealed class Databases
        {
            public const string AvatarDb = "avatar.db";
            public const string d23Db = "d23.db";
            public const string EventsDb = "events.db";
            public const string IdentityAccessDb = "identity-access.db";
            public const string SubscriptionsDb = "subscriptions.db";
        }

        public sealed class Titles
        {
            public const string AvatarInstaller = "Avatar Installer";
            public const string NeuronPermit = "Neuron Permit";
            public const string RegionPermit = "Region Permit";
            public const string User = "User";
        }

        public sealed class Statuses
        {
            public const string Cancelled = "Cancelled";
            public const string Success = "Success";
            public const string Error = "Error";
            public const string Invalid = "Invalid";
            public const string Update = "Update";
        }

        public sealed class Prompts
        {
            public const string Ok = "Ok";
            public const string Yes = "Yes";
            public const string No = "No";
        }

        public sealed class Urls
        {
            public const string DefaultTemplateDownloadUrl = "https://github.com/ei8/avatar-template.git";
        }

        public sealed class BatchFileTemplates
        {
            public const string StartTraefikBat = @"traefik_windows-amd64.exe -c {0}";
            public const string StartEi8SiteBat = "@echo off\n" +
                                                  "@echo ___________________________________________\n" +
                                                  "@echo Started: %date% %time%\n" +
                                                  "ssh -F {0} ei8.site";

            public const string LoopStartEi8SiteBat = "@echo off\n" +
                                                      "@echo Loop-starting ... \n" +
                                                     @"FOR /L %%N IN () DO call ""start - ei8.site.bat""";

            public const string AutoLockCmd = "timeout /t 10\n" +
                                               "rundll32.exe user32.dll,LockWorkStation";
        }
    }
}
