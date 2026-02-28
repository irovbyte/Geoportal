namespace Geoportal.Pages;

public partial class DataTablePage : ContentPage
{
	public DataTablePage()
	{
		InitializeComponent();
	}
    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}