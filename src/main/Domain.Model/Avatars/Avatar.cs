using ei8.Cortex.Coding.Properties;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public class Avatar
    {
        [neurULNeuronProperty]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [neurULNeuronProperty]
        public string MirrorUrl { get; set; }

        [neurULNeuronProperty]
        public string Url { get; set; }
    }
}
