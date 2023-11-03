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

            public const string AutoLockCmd =  "timeout /t 10\n" +
                                               "rundll32.exe user32.dll,LockWorkStation";
        }
    }
}
