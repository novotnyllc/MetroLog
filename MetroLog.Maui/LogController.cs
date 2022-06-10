using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MetroLog.Operators;

namespace MetroLog.Maui;


public class LogController : INotifyPropertyChanged
{
    private static readonly List<LogController> LogControllers = new ();

    private static bool _isShakeEnabled;

    private static bool _suspendedShakeEnabledValue;

    private static Func<Page, Task>? _globalNavigationFunction;

    public LogController()
    {
        OperatorRetriever = LogOperatorRetriever.Instance;

        ToggleShakeCommand = new Command(() => IsShakeEnabled = !IsShakeEnabled);
        GoToLogsPageCommand = new Command(GoToLogsPage);

        LogControllers.Add(this);

        OperatorRetriever.TryGetOperator<ILogCompressor>(out var logCompressor);
        LogCompressor = logCompressor;

        OperatorRetriever.TryGetOperator<ILogLister>(out var logStringifier);
        LogLister = logStringifier;
    }

    public static void InitializeNavigation(Func<Page, Task> navigationFunction)
    {
        _globalNavigationFunction = navigationFunction;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand ToggleShakeCommand { get; }

    public ICommand GoToLogsPageCommand { get; }

    public bool CanGetCompressedLogs => LogCompressor != null;

    public bool CanGetLogsString => LogLister != null;

    public bool IsShakeEnabled
    {
        get => _isShakeEnabled;
        set
        {
            if (_isShakeEnabled != value)
            {
                ToggleAccelerometer(value);
                _isShakeEnabled = value;
            }

            OnPropertyChanged();
        }
    }

    protected ILogCompressor? LogCompressor { get; }

    protected ILogLister? LogLister { get; }

    protected ILogOperatorRetriever OperatorRetriever { get; }

    public static void SuspendShake()
    {
        _suspendedShakeEnabledValue = _isShakeEnabled;

        foreach (var logController in LogControllers)
        {
            logController.IsShakeEnabled = false;
        }
    }

    public static void ResumeShakeIfNeeded()
    {
        if (!_suspendedShakeEnabledValue)
        {
            return;
        }

        foreach (var logController in LogControllers)
        {
            logController.IsShakeEnabled = true;
        }
    }

    public async Task<MemoryStream?> GetCompressedLogs()
    {
        if (LogCompressor == null)
        {
            return null;
        }

        return await LogCompressor.GetCompressedLogs();
    }

    public async Task<List<string>?> GetLogList()
    {
        if (LogLister == null)
        {
            return null;
        }

        return await LogLister.GetLogList();
    }

    private static void ToggleAccelerometer(bool enable)
    {
        SensorSpeed speed = SensorSpeed.Game;
        try
        {
            if (enable && !Accelerometer.IsMonitoring)
            {
                Accelerometer.Start(speed);
                Accelerometer.ShakeDetected += AccelerometerOnShakeDetected;
            }
            else
            {
                Accelerometer.Stop();
                Accelerometer.ShakeDetected -= AccelerometerOnShakeDetected;
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Feature not supported on device
        }
        catch (Exception ex)
        {
            // Other error has occurred.
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static async void AccelerometerOnShakeDetected(object? sender, EventArgs e)
    {
        if (_globalNavigationFunction == null)
        {
            return;
        }

        await _globalNavigationFunction!.Invoke(new MetroLogPage());
    }

    private async void GoToLogsPage()
    {
        if (_globalNavigationFunction == null)
        {
            throw new InvalidOperationException(
                "You first need to initialize global navigation function by calling LogController.InitializeNavigation");
        }

        await _globalNavigationFunction(new MetroLogPage());
    }
}
