using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public class NeuronPermitDto
{
    public string UserNeuronId { get; set; }
    public string NeuronId { get; set; }
    public string ExpirationDate { get; set; }
}

public class RegionPermitDto
{
    public int SequenceId { get; set; }
    public Guid UserNeuronId { get; set; }
    public Guid RegionNeuronId { get; set; }
    public int WriteLevel { get; set; }
    public int ReadLevel { get; set; }
}

public class UserDto
{
    public string UserId { get; set; }
    public Guid NeuronId { get; set; }
    public int? Active { get; set; }
}

//public class SQLiteSequenceDto
//{
//    public object Name { get; set; }
//    public object Seq { get; set; }
//}
