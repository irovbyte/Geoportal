using System.Globalization;
using Geoportal.Resources.Languages;

namespace Geoportal.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        UserPhoneLabel.Text = Preferences.Default.Get("user_phone", "+998 00 000 00 00");
    }

    private void OnLangUzClicked(object sender, EventArgs e) => ChangeLanguage("uz");
    private void OnLangRuClicked(object sender, EventArgs e) => ChangeLanguage("ru");
    private void OnLangEnClicked(object sender, EventArgs e) => ChangeLanguage("en");

    private async void ChangeLanguage(string langCode)
    {
        this.IsBusy = true;
        await Task.Delay(500);

        Preferences.Default.Set("app_lang", langCode);

        var culture = new CultureInfo(langCode);
        AppResources.Culture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
            await Shell.Current.GoToAsync("//SettingsPage");
        }

        this.IsBusy = false;
    }

    private async void OnViewReportsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateReportPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(AppResources.Logout, "Вы уверены?", "Да", "Нет");
        if (confirm)
        {
            Preferences.Default.Set("is_logged_in", false);
            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
            }
        }
    }
}