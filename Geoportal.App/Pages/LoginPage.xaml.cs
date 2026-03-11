using Geoportal.Resources.Languages;
using Geoportal.Services;
using Geoportal.Service.Helpers;

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
        // 1. Мгновенная подготовка (скрываем карточку)
        AnimationHelper.Prepare(LoginCard);

        base.OnAppearing();

        // 2. Ждем один цикл отрисовки
        await Task.Yield();

        // 3. Запускаем входную анимацию карточки (Зум + Fade + Подъем)
        await AnimationHelper.EntranceAsync(LoginCard);

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
        // Клик-анимация через хелпер
        await AnimationHelper.ExecuteClickScaleAsync(SubmitBtn);

        string rawPhone = "+998" + PhoneEntry.Text;

        if (string.IsNullOrEmpty(PhoneEntry.Text) || PhoneEntry.Text.Length < 9 || (PasswordEntry.Text?.Length ?? 0) < 6)
        {
            await DisplayAlert(AppResources.ErrorTitle, AppResources.ErrorMessage, "OK");
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
        if (sender is View view) await AnimationHelper.ExecuteClickScaleAsync(view);

        _isLoginMode = !_isLoginMode;

        // Плавная смена режимов
        await Task.WhenAll(
            TitleLabel.FadeTo(0, 150),
            SubmitBtn.FadeTo(0, 150)
        );

        TitleLabel.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SubmitBtn.Text = _isLoginMode ? AppResources.LoginTitle : AppResources.RegisterTitle;
        SwitchBtn.Text = _isLoginMode ? AppResources.SwitchToRegister : AppResources.SwitchToLogin;

        if (!_isLoginMode)
        {
            ConfirmBorder.IsVisible = true;
            await ConfirmBorder.FadeTo(1, 250, Easing.CubicOut);
        }
        else
        {
            await ConfirmBorder.FadeTo(0, 200);
            ConfirmBorder.IsVisible = false;
        }

        await Task.WhenAll(
            TitleLabel.FadeTo(1, 150),
            SubmitBtn.FadeTo(1, 150)
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