using Geoportal.Service.Helpers;

namespace Geoportal.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
    }

    protected override async void OnAppearing()
    {
        // 1. Мгновенно готовим элементы (прячем)
        AnimationHelper.Prepare(MainContainer, HeaderGroup, ActionButtons, StatsCards, SummaryInfo, BottomFilters);

        base.OnAppearing();

        // 2. Даем отрисоваться
        await Task.Yield();

        // 3. Запускаем анимацию входа (Зум + Fade + Подъем)
        await AnimationHelper.EntranceAsync(MainContainer);

        // 4. Каскадное появление остальных частей
        await AnimationHelper.EntranceAsync(HeaderGroup, 100);
        await AnimationHelper.EntranceAsync(ActionButtons, 100);
        await AnimationHelper.EntranceAsync(StatsCards, 100);
        await AnimationHelper.EntranceAsync(SummaryInfo, 100);
        await AnimationHelper.EntranceAsync(BottomFilters, 100);
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