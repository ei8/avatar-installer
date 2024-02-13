using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public class ResourceDto
{
    public string PathPattern { get; set; }
    public string? InUri { get; set; }
    public string? OutUri { get; set; }
    public string? Methods { get; set; }
}
