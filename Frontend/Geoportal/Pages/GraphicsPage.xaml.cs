namespace Geoportal.Pages;

public partial class GraphicsPage : ContentPage
{
	public GraphicsPage()
	{
		InitializeComponent();
	}
    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}