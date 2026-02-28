namespace Geoportal.Pages;

public partial class CreateReportPage : ContentPage
{
	public CreateReportPage()
	{
		InitializeComponent();
	}
    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    // Обработчик для выбора фото (пунктирная зона)
    private async void OnAddPhotoTapped(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Выберите фото инфраструктуры",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            // Здесь будет логика отображения выбранного фото
        }
    }
}