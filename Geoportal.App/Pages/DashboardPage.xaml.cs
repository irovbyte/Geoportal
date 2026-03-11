namespace Geoportal.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly ApiService _apiService;
    public DashboardPage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        AnimationHelper.Prepare(MainContainer, HeaderGroup, ActionButtons, StatsCards, SummaryInfo, BottomFilters);

        base.OnAppearing();

        await Task.Yield();

        await AnimationHelper.EntranceAsync(MainContainer);

        await AnimationHelper.EntranceAsync(HeaderGroup, 100);
        await AnimationHelper.EntranceAsync(ActionButtons, 100);
        await AnimationHelper.EntranceAsync(StatsCards, 100);
        await AnimationHelper.EntranceAsync(SummaryInfo, 100);
        await AnimationHelper.EntranceAsync(BottomFilters, 100);
    }
    private async Task LoadDashboardDataAsync()
    {
        var data = await _apiService.GetDashboardSummaryAsync();

        if (data != null && data.Totals != null)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                LblAllocated.Text = (data.Totals.Allocated / 1_000_000_000m).ToString("N2");
                LblSpent.Text = (data.Totals.Spent / 1_000_000_000m).ToString("N2");
                LblProjects.Text = data.Totals.ProjectsCount.ToString("N0");
                LblMastery.Text = $"{data.Totals.Completion}%";
            });
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