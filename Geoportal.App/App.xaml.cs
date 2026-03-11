using Geoportal.Pages;
using Geoportal.Resources.Languages;
using System.Globalization;

namespace Geoportal;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // 1. Установка языка остается в конструкторе
        string deviceLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        string savedLang = Preferences.Default.Get("app_lang", deviceLanguage);

        var culture = new CultureInfo(savedLang);
        AppResources.Culture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {

        bool isLoggedIn = Preferences.Default.Get("is_logged_in", false);

        Page rootPage;

        if (isLoggedIn)
        {
            rootPage = new AppShell();
        }
        else
        {
            rootPage = new LoginPage();
        }

        return new Window(rootPage);
    }
}