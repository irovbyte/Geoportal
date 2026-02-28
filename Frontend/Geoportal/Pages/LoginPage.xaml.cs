using Geoportal.Resources.Languages;
using Geoportal.Services;

namespace Geoportal.Pages;

public partial class LoginPage : ContentPage
{
    private bool _isLoginMode = true;
    private readonly AuthService _authService = new();
    private bool _isFormatting = false;

    public LoginPage()
    {
        InitializeComponent();
        CheckAutoLogin();
    }

    private void CheckAutoLogin()
    {
        if (Preferences.Default.Get("is_logged_in", false))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var app = Application.Current;
                if (app?.Windows.Count > 0)
                {
                    app.Windows[0].Page = new Geoportal.AppShell();
                }
            });
        }
    }
    private void OnPhoneTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isFormatting) return;

        var entry = sender as Entry;
        if (entry == null) return;

        _isFormatting = true;

        try
        {
            string cleaned = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());

            if (cleaned.Length > 9)
                cleaned = cleaned.Substring(0, 9);

            if (entry.Text != cleaned)
                entry.Text = cleaned;
        }
        finally
        {
            _isFormatting = false;
        }
    }
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        string rawPhone = "+998" + new string(PhoneEntry.Text?.Where(char.IsDigit).ToArray());

        if (rawPhone.Length < 12 || PasswordEntry.Text?.Length < 6)
        {
            await DisplayAlert(AppResources.ErrorTitle, AppResources.ErrorMessage, "OK");
            return;
        }

        if (!_isLoginMode && PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert(AppResources.ErrorTitle, AppResources.PassMismatch, "OK");
            return;
        }

        SetLoading(true);

        var result = _isLoginMode
            ? await _authService.LoginAsync(rawPhone, PasswordEntry.Text ?? "")
            : await _authService.RegisterAsync(rawPhone, PasswordEntry.Text ?? "");

        SetLoading(false);

        if (result.Success)
        {

            Preferences.Default.Set("is_logged_in", true);

            Preferences.Default.Set("user_phone", rawPhone);

     
            if (!Preferences.Default.ContainsKey("app_lang"))
            {
                var deviceLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                Preferences.Default.Set("app_lang", deviceLanguage);
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var app = Application.Current;
                if (app?.Windows.Count > 0)
                {

                    app.Windows[0].Page = new Geoportal.AppShell();
                }
            });
        }
    }

    private async void OnSwitchModeClicked(object sender, EventArgs e)
    {
        _isLoginMode = !_isLoginMode;

        if (!_isLoginMode) ConfirmBorder.IsVisible = true;

        await Task.WhenAll(
            TitleLabel.FadeTo(0, 100),
            SubmitBtn.FadeTo(0, 100),
            ConfirmBorder.FadeTo(_isLoginMode ? 0 : 1, 200)
        );

        TitleLabel.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SubmitBtn.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SwitchBtn.Text = _isLoginMode ? AppResources.SwitchToRegister : AppResources.SwitchToLogin;

        if (_isLoginMode) ConfirmBorder.IsVisible = false;

        await Task.WhenAll(TitleLabel.FadeTo(1, 100), SubmitBtn.FadeTo(1, 100));
    }

    private void SetLoading(bool isLoading)
    {
        Loader.IsVisible = isLoading;
        Loader.IsRunning = isLoading;
        SubmitBtn.IsEnabled = !isLoading;
    }
}