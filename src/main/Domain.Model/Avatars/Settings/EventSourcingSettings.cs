using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class EventSourcingSettings
{
    public string DatabasePath { get; set; }
    public bool DisplayErrorTraces { get; set; }
}
