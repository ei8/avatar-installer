using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application;

public interface IProgressService
{
    event EventHandler ProgressChanged;
    event EventHandler DescriptionChanged;

    double Progress { get; }
    string Description { get; }

    void Update(double value, string description);
    void Reset();
}
