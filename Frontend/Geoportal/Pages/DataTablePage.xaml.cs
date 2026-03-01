namespace Geoportal.Pages;

using Geoportal.Service.Helpers;

public partial class DataTablePage : ContentPage
{
    public DataTablePage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
    }

    protected override async void OnAppearing()
    {
        // 1. Прячем нужные блоки
        AnimationHelper.Prepare(MainContainer, RowsContainer);

        base.OnAppearing();
        await Task.Yield();

        // 2. Запускаем анимацию входа (теперь она везде одинаковая)
        await AnimationHelper.EntranceAsync(MainContainer);

        // Если хочешь каскад (чтобы таблица вылетала чуть позже)
        await AnimationHelper.EntranceAsync(RowsContainer, 100);
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
        // Для Windows делаем плавный зум, чтобы текст не дергался при ресайзе
        if (MainContainer != null)
        {
            double targetScale = Width > 1200 ? 1.05 : 1.0;
            if (MainContainer.Scale != targetScale)
                MainContainer.ScaleToAsync(targetScale, 250, Easing.CubicOut);
        }
    }
}