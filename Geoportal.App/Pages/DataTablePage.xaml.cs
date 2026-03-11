namespace Geoportal.Pages;
public partial class DataTablePage : ContentPage
{
    public DataTablePage()
    {
        InitializeComponent();
        this.SizeChanged += OnPageSizeChanged;
    }

    protected override async void OnAppearing()
    {
        AnimationHelper.Prepare(MainContainer, RowsContainer);

        base.OnAppearing();
        await Task.Yield();
        await AnimationHelper.EntranceAsync(MainContainer);
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
        if (MainContainer != null)
        {
            double targetScale = Width > 1200 ? 1.05 : 1.0;
            if (MainContainer.Scale != targetScale)
                MainContainer.ScaleToAsync(targetScale, 250, Easing.CubicOut);
        }
    }
}