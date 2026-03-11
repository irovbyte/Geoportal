namespace Geoportal;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Pages.CreateReportPage), typeof(Pages.CreateReportPage));
        Routing.RegisterRoute(nameof(Pages.LoginPage), typeof(Pages.LoginPage));
    }

}