using MetroLog.Maui;
using Microsoft.Extensions.Logging;

namespace MetroLogSample.Maui;

public partial class MainPage : ContentPage
{
    private readonly ILogger<MainPage> _logger;
    private int _count;

    public MainPage(ILogger<MainPage> logger)
    {
        _logger = logger;
        InitializeComponent();

        BindingContext = new LogController();
        NavigationPage.SetHasNavigationBar(this, false);

        RandomLogs();
    }

    private LogController LogController => (LogController)BindingContext;

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _logger.LogInformation("OnCounterClicked()");

        _count++;

        if (_count == 1)
        {
            CounterBtn.Text = $"Clicked {_count} time";
        }
        else
        {
            CounterBtn.Text = $"Clicked {_count} times";
        }

        _logger.LogInformation($"we have clicked {_count} times!");

        if (_count % 10 == 0)
        {
            _logger.LogWarning("WOW 10x times");
        }

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void RandomLogs()
    {
        while (true)
        {
            await Task.Delay(500);
            double time = DateTime.Now.Millisecond;
            if (time % 2 == 0)
            {
                _logger.LogInformation("Date time milliseconds is even");
            }
            else if (time % 3 == 0)
            {
                _logger.LogWarning("Some random logs are warnings...");
            }
            else if (time % 5 == 0)
            {
                _logger.LogError("Some random logs are ERRORS...");
            }
            else
            {
                _logger.LogInformation("Everything is fine and boring.");
            }
        }
    }
}
