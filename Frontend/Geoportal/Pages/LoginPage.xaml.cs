using Geoportal.Resources.Languages;
using GeoportalApp.Services;

namespace GeoportalApp.Pages;

public partial class LoginPage : ContentPage
{
    private bool _isLoginMode = true;
    private readonly AuthService _authService = new();

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
                    app.Windows[0].Page = new NavigationPage(new MainPage());
                }
            });
        }
    }

    private void OnPhoneTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue)) return;

        string cleaned = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        string formatted = "";
        for (int i = 0; i < cleaned.Length; i++)
        {
            if (i == 2 || i == 5 || i == 7) formatted += " ";
            formatted += cleaned[i];
        }

        if (e.OldTextValue != formatted)
        {
            ((Entry)sender).Text = formatted;
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
            string message = _isLoginMode ? AppResources.WelcomeMessage : AppResources.RegisterSuccess;
            await DisplayAlert(AppResources.SuccessTitle, message, "OK");

            if (_isLoginMode)
            {
                Preferences.Default.Set("is_logged_in", true);
            }
        }
        else
        {
            try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500)); } catch { }
            await DisplayAlert(AppResources.ErrorTitle, result.Message, "OK");
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