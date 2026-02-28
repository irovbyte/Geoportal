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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Входная анимация страницы через Async
        this.Opacity = 0;
        await this.FadeToAsync(1, 400, Easing.CubicOut);
        CheckAutoLogin();
    }

    private void CheckAutoLogin()
    {
        if (Preferences.Default.Get("is_logged_in", false))
        {
            Application.Current.MainPage = new Geoportal.AppShell();
        }
    }

    private void OnPhoneTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isFormatting) return;
        _isFormatting = true;
        try
        {
            string cleaned = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());
            if (cleaned.Length > 9) cleaned = cleaned.Substring(0, 9);
            PhoneEntry.Text = cleaned;
        }
        finally { _isFormatting = false; }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        // Клик-анимация кнопки
        await SubmitBtn.ScaleToAsync(0.96, 70, Easing.CubicIn);
        await SubmitBtn.ScaleToAsync(1.0, 70, Easing.CubicOut);

        string rawPhone = "+998" + PhoneEntry.Text;

        if (string.IsNullOrEmpty(PhoneEntry.Text) || PhoneEntry.Text.Length < 9 || (PasswordEntry.Text?.Length ?? 0) < 6)
        {
            await DisplayAlertAsync(AppResources.ErrorTitle, AppResources.ErrorMessage, "OK");
            return;
        }

        SetLoading(true);
        var result = _isLoginMode
            ? await _authService.LoginAsync(rawPhone, PasswordEntry.Text)
            : await _authService.RegisterAsync(rawPhone, PasswordEntry.Text);
        SetLoading(false);

        if (result.Success)
        {
            Preferences.Default.Set("is_logged_in", true);
            Preferences.Default.Set("user_phone", rawPhone);
            Application.Current.MainPage = new Geoportal.AppShell();
        }
    }

    private async void OnSwitchModeClicked(object sender, EventArgs e)
    {
        _isLoginMode = !_isLoginMode;

        // Плавное скрытие старых текстов
        await Task.WhenAll(
            TitleLabel.FadeToAsync(0, 150, Easing.SinIn),
            SubmitBtn.FadeToAsync(0, 150, Easing.SinIn)
        );

        TitleLabel.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SubmitBtn.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SwitchBtn.Text = _isLoginMode ? AppResources.SwitchToRegister : AppResources.SwitchToLogin;

        if (!_isLoginMode)
        {
            ConfirmBorder.IsVisible = true;
            await ConfirmBorder.FadeToAsync(1, 250, Easing.CubicOut);
        }
        else
        {
            await ConfirmBorder.FadeToAsync(0, 200, Easing.SinIn);
            ConfirmBorder.IsVisible = false;
        }

        // Плавное появление новых текстов
        await Task.WhenAll(
            TitleLabel.FadeToAsync(1, 150, Easing.SinOut),
            SubmitBtn.FadeToAsync(1, 150, Easing.SinOut)
        );
    }

    private void SetLoading(bool isLoading)
    {
        Loader.IsVisible = isLoading;
        Loader.IsRunning = isLoading;
        SubmitBtn.IsEnabled = !isLoading;
        SubmitBtn.Opacity = isLoading ? 0.6 : 1.0;
    }
}