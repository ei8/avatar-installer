using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public class ViewDto
{
    public string Url { get; set; }
    public string? ParentUrl { get; set; }
    public string? Name { get; set; }
    public int IsDefault { get; set; }
    public int Sequence { get; set; }
    public string? Icon { get; set; }
}
