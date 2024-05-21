namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings
{
    public class CortexGraphSettings
    {
        public int PollInterval { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string DbUrl { get; set; }
        public int DefaultRelativeValues { get; set; }
        public int DefaultNeuronActiveValues { get; set; }
        public int DefaultTerminalActiveValues { get; set; }
        public int DefaultPageSize { get; set; }
        public int DefaultPage { get; set; }
        public string ArangoRootPassword { get; set; }
    }
}
