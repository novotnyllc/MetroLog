using MetroLog.Maui;

namespace MetroLogSample.Maui;

public partial class App : Application
{
    public App(MainPage mainPage)
    {
        InitializeComponent();

        MainPage = new NavigationPage(mainPage);

        LogController.InitializeNavigation(
            page => MainPage!.Navigation.PushModalAsync(page));
    }
}
