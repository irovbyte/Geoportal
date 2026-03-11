using Geoportal.Service.Helpers;

namespace Geoportal.Pages;

public partial class CreateReportPage : ContentPage
{
    public CreateReportPage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
    }

    protected override async void OnAppearing()
    {
        // 1. Мгновенная подготовка (скрываем всё)
        AnimationHelper.Prepare(MainContainer, HeaderRow, PhotoZone, FormGroup, SubmitBtn);

        base.OnAppearing();

        // 2. Ждем один цикл отрисовки
        await Task.Yield();

        // 3. Плавный вход основной страницы
        await AnimationHelper.EntranceAsync(MainContainer);

        // 4. Последовательный вылет элементов формы
        await AnimationHelper.EntranceAsync(HeaderRow, 50);
        await AnimationHelper.EntranceAsync(PhotoZone, 100);
        await AnimationHelper.EntranceAsync(FormGroup, 100);
        await AnimationHelper.EntranceAsync(SubmitBtn, 100);
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
            // Плавный зум для больших экранов
            double targetScale = Width > 1200 ? 1.05 : 1.0;
            if (MainContainer.Scale != targetScale)
                MainContainer.ScaleTo(targetScale, 250, Easing.CubicOut);
        }
    }

    private async void OnAddPhotoTapped(object sender, EventArgs e)
    {
        if (sender is View view)
        {
            await AnimationHelper.ExecuteClickScaleAsync(view);
        }

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Выберите фото инфраструктуры",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            // Логика обработки фото
        }
    }
}