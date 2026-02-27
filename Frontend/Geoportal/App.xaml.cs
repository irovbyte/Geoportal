using GeoportalApp.Pages;

namespace Geoportal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new LoginPage());
        }

    }
}