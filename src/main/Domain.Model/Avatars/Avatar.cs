using ei8.Cortex.Coding.Properties;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    public class Avatar
    {
        public string Name { get; set; }

        public DateTimeOffset? CreationTimestamp { get; set; }

        public DateTimeOffset? LastModificationTimestamp { get; set; }

        #region Neuron Properties
        [neurULNeuronProperty]
        public Guid Id { get; set; }

        [neurULNeuronProperty]
        public string MirrorUrl { get; set; }

        [neurULNeuronProperty]
        public string Url { get; set; }
        #endregion
    }
}
