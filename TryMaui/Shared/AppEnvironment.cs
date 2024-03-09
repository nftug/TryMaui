namespace TryMaui.Shared;


public static class AppEnvironment
{
    public static IServiceProvider Services =>
        Application.Current!.Handler.MauiContext!.Services;
}
