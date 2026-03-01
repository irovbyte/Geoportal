using System.Globalization;
using Geoportal.Resources.Languages;
using Geoportal.Service.Helpers;

namespace Geoportal.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
        UserPhoneLabel.Text = Preferences.Default.Get("user_phone", "+998 00 000 00 00");
    }

    protected override async void OnAppearing()
    {
        // 1. Мгновенная подготовка
        AnimationHelper.Prepare(MainContainer, SettingsTitle, ProfileCard, ReportsCard, LangGroup, LogoutBtn);

        base.OnAppearing();
        await Task.Yield();

        // 2. Вход страницы
        await AnimationHelper.EntranceAsync(MainContainer);

        // 3. Каскад элементов
        await AnimationHelper.EntranceAsync(SettingsTitle, 50);
        await AnimationHelper.EntranceAsync(ProfileCard, 100);
        await AnimationHelper.EntranceAsync(ReportsCard, 100);
        await AnimationHelper.EntranceAsync(LangGroup, 100);
        await AnimationHelper.EntranceAsync(LogoutBtn, 100);
    }

    private async void OnLangUzClickedAsync(object sender, EventArgs e) => await ChangeLanguageAsync("uz");
    private async void OnLangRuClickedAsync(object sender, EventArgs e) => await ChangeLanguageAsync("ru");
    private async void OnLangEnClickedAsync(object sender, EventArgs e) => await ChangeLanguageAsync("en");

    private async Task ChangeLanguageAsync(string langCode)
    {
        try
        {
            // 1. Показываем загрузку через анимацию
            LoadingOverlay.IsVisible = true;
            await Task.WhenAll(
                MainContent.FadeToAsync(0, 200),
                LoadingOverlay.FadeToAsync(1, 200)
            );

            // 2. Логика смены языка
            Preferences.Default.Set("app_lang", langCode);
            var culture = new CultureInfo(langCode);
            AppResources.Culture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await Task.Delay(400);

            if (Application.Current?.Windows.Count > 0)
            {
                var newShell = new AppShell();
                Application.Current.Windows[0].Page = newShell;

                // Переключаемся на вкладку настроек в новом Shell
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
            await DisplayAlertAsync("Error", ex.Message, "OK");
            LoadingOverlay.IsVisible = false;
            MainContent.Opacity = 1;
        }
    }

    private async void OnViewReportsClickedAsync(object sender, EventArgs e)
    {
        if (sender is View view) await AnimationHelper.ExecuteClickScaleAsync(view);
        await Shell.Current.GoToAsync(nameof(CreateReportPage));
    }

    private async void OnLogoutClickedAsync(object sender, EventArgs e)
    {
        if (sender is View view) await AnimationHelper.ExecuteClickScaleAsync(view);

        bool confirm = await DisplayAlertAsync(AppResources.Logout, "Вы уверены?", "Да", "Нет");
        if (confirm)
        {
            Preferences.Default.Set("is_logged_in", false);
            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
            }
        }
    }

    private void OnPageSizeChanged(object? sender, EventArgs e)
    {
        if (MainContainer != null)
        {
            double targetScale = Width > 1200 ? 1.05 : 1.0;
            if (MainContainer.Scale != targetScale)
                MainContainer.ScaleToAsync(targetScale, 250, Easing.CubicOut);
        }
    }
}