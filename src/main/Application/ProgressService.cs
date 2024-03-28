using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application;

public class ProgressService : IProgressService
{
    public event EventHandler ProgressChanged;
    public event EventHandler DescriptionChanged;

    private double progress;
    private string description;

    public ProgressService()
    {
        this.Reset();
    }

    public double Progress
    {
        get => this.progress;
        set
        {
            if (this.progress != value)
            {
                this.progress = value;
                if (this.ProgressChanged is not null)
                    this.ProgressChanged(this, EventArgs.Empty);
            }
        }
    }

    public string Description
    {
        get => this.description;
        set
        {
            if (this.description != value)
            {
                this.description = value;

                if (this.DescriptionChanged is not null)
                    this.DescriptionChanged(this, EventArgs.Empty);
            }
        }
    }

    public void Reset()
    {
        this.Update(0, string.Empty);
    }

    public void Update(double value, string description)
    {
        this.Progress = value;
        this.Description = description;
    }
}
