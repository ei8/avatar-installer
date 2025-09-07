using ei8.Cortex.Coding.Properties;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public class Avatar
    {
        public string Name { get; set; }

        [neurULNeuronProperty]
        public Guid Id { get; set; }
    }
}
