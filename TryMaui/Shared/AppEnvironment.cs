namespace TryMaui.Shared;


public static class AppEnvironment
{
    public static IServiceProvider Services =>
#if WINDOWS10_0_17763_0_OR_GREATER
    MauiWinUIApplication.Current.Services;
#elif ANDROID
MauiApplication.Current.Services;
#elif IOS || MACCATALYST
    MauiUIApplicationDelegate.Current.Services;
#else
    null;
#endif
}

