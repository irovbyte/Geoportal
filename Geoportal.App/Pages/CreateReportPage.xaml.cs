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
        AnimationHelper.Prepare(MainContainer, HeaderRow, PhotoZone, FormGroup, SubmitBtn);

        base.OnAppearing();
        await Task.Yield();
        await AnimationHelper.EntranceAsync(MainContainer);
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
            double targetScale = Width > 1200 ? 1.05 : 1.0;
            if (MainContainer.Scale != targetScale)
                MainContainer.ScaleToAsync(targetScale, 250, Easing.CubicOut);
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
        }
    }
}