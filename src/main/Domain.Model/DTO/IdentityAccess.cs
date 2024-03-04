using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public record NeuronPermit(string UserNeuronId, string NeuronId, string? ExpirationDate);
public record RegionPermit(int SequenceId, string? UserNeuronId, string? RegionNeuronId, int? WriteLevel, int? ReadLevel);
public record User(string UserId, string NeuronId, int? Active);
