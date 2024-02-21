
namespace Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window =  base.CreateWindow(activationState);

        //window.X = 500;
        //window.Y = 200;

        //const int desiredWidth = 1000;
        //const int desiredHeight = 800;

        //window.Width = desiredWidth;
        //window.Height = desiredHeight;

        //window.MinimumWidth = desiredWidth;
        //window.MinimumHeight = desiredHeight;
        //window.MaximumWidth = desiredWidth;
        //window.MaximumHeight = desiredHeight;

        return window;
    }
}
