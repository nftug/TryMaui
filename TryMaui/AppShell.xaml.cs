namespace TryMaui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

    void Shell_Loaded(System.Object sender, System.EventArgs e)
    {
        Window.MinimumWidth = 1000;
        Window.MaximumWidth = 1000;
        Window.MinimumHeight = 700;
        Window.MaximumHeight = 700;

        // Give the Window time to resize
        Dispatcher.Dispatch(() =>
        {
            Window.MinimumWidth = 0;
            Window.MinimumHeight = 0;
            Window.MaximumWidth = double.PositiveInfinity;
            Window.MaximumHeight = double.PositiveInfinity;
        });
    }
}

