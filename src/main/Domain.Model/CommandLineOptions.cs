namespace ei8.Avatar.Installer.Domain.Model
{
    public class CommandLineOptions
    {
        public string DestinationPath { get; }

        public CommandLineOptions(string[] args)
        {
            foreach (var arg in args)
            {
                var argTokens = arg.Split('=');
                var argName = argTokens[0];
                var argValue = argTokens[1];

                switch (argName)
                {
                    case "-d":
                    case "--destination":
                        DestinationPath = argValue;
                        break;
                }
            }
        }
    }
}
