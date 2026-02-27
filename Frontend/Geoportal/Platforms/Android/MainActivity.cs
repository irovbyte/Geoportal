using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Geoportal
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Window != null)
            {
#pragma warning disable CA1416
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                    SystemUiFlags.LayoutFullscreen |
                    SystemUiFlags.LayoutStable |
                    SystemUiFlags.LayoutHideNavigation
                );
#pragma warning restore CA1416
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (Window != null)
            {
#pragma warning disable CA1416
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                    SystemUiFlags.LayoutFullscreen |
                    SystemUiFlags.LayoutStable
                );
#pragma warning restore CA1416
            }
        }
    }
}