﻿using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using neurUL.Common.Domain.Model;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    /// <summary>
    /// A concrete representation of an avatar, derived from a <see cref="Configuration.AvatarConfiguration"/>
    /// </summary>
    public class AvatarItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }

        // TODO: Change to AvatarSettings class
        public EventSourcingSettings EventSourcing { get; set; } = new();
        public CortexGraphSettings CortexGraph { get; set; } = new();
        public AvatarApiSettings AvatarApi { get; set; } = new();
        public IdentityAccessSettings IdentityAccess { get; set; } = new();
        public CortexLibrarySettings CortexLibrary { get; set; } = new();
        public CortexDiaryNucleusSettings CortexDiaryNucleus { get; set; } = new();
        // END TODO 

        public d23Settings d23 { get; set; } = new();
        public AvatarNetworkSettings Network { get; set; } = new();

        public AvatarItem(string id, string name)
        {
            AssertionConcern.AssertArgumentNotEmpty(id, "Specified 'id' cannot be empty.", nameof(id));
            AssertionConcern.AssertArgumentNotNull(id, nameof(id));

            AssertionConcern.AssertArgumentNotEmpty(name, "Specified 'name' cannot be empty.", nameof(name));
            AssertionConcern.AssertArgumentNotNull(name, nameof(name));

            this.Id = id;
            this.Name = name;
        }
    }
}
