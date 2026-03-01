using Geoportal.Service.Helpers;

namespace Geoportal.Pages;

public partial class GraphicsPage : ContentPage
{
    public GraphicsPage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
    }

    protected override async void OnAppearing()
    {
        // 1. Прячем всё через Prepare
        AnimationHelper.Prepare(MainContainer, HeaderRow, FilterRow, ChartContainer, ChartLegend, Bar1, Bar2, Bar3);

        base.OnAppearing();
        await Task.Yield();

        // 2. Вход страницы
        await AnimationHelper.EntranceAsync(MainContainer);

        // 3. Каскад заголовка и фильтров
        await AnimationHelper.EntranceAsync(HeaderRow, 50);
        await AnimationHelper.EntranceAsync(FilterRow, 50);

        // 4. Вход самого контейнера графика
        await AnimationHelper.EntranceAsync(ChartContainer, 50);
        await AnimationHelper.EntranceAsync(ChartLegend, 50);

        // 5. ЭФФЕКТ ДОМИНО: Столбцы графика вылетают по очереди
        await AnimationHelper.EntranceAsync(Bar1, 100);
        await AnimationHelper.EntranceAsync(Bar2, 100);
        await AnimationHelper.EntranceAsync(Bar3, 100);
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        if (sender is View view)
        {
            await AnimationHelper.ExecuteClickScaleAsync(view);
        }
        await Shell.Current.GoToAsync("..");
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