namespace GeoportalApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        NavigateTo(new DashboardView());
    }

    private void OnDashboardTapped(object sender, EventArgs e) => NavigateTo(new DashboardView());
    private void OnReportTapped(object sender, EventArgs e) => NavigateTo(new ReportView());
    private void OnCompareTapped(object sender, EventArgs e) => NavigateTo(new CompareView());
    private void OnSettingsTapped(object sender, EventArgs e) => NavigateTo(new SettingsView());

    private void NavigateTo(ContentView view)
    {
        if (MainContent.Content?.GetType() == view.GetType()) return;

        view.Opacity = 0;
        MainContent.Content = view;
        view.FadeTo(1, 250);
    }
}