using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ContextMenu;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static Activity Instance { get; private set; } = default!;

    public MainActivity()
    {
        Instance = this;

    }
}