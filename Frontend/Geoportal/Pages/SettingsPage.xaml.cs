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
        try
        {
            // 1. Показываем загрузку и приглушаем контент
            LoadingOverlay.IsVisible = true;
            await Task.WhenAll(
                MainContent.FadeTo(0, 200),
                LoadingOverlay.FadeTo(1, 200)
            );

            // 2. Логика смены языка
            Preferences.Default.Set("app_lang", langCode);
            var culture = new System.Globalization.CultureInfo(langCode);
            Geoportal.Resources.Languages.AppResources.Culture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

            // 3. Небольшая пауза для эффекта
            await Task.Delay(400);

            if (Application.Current?.Windows.Count > 0)
            {
                var newShell = new AppShell();

                // Устанавливаем новый Shell
                Application.Current.Windows[0].Page = newShell;

                // Находим вкладку настроек в новом Shell
                var settingsTab = newShell.Items
                    .SelectMany(x => x.Items)
                    .FirstOrDefault(x => x.Route == "SettingsPage");

                if (settingsTab != null)
                {
                    newShell.CurrentItem = settingsTab;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            LoadingOverlay.IsVisible = false;
            MainContent.Opacity = 1;
        }
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