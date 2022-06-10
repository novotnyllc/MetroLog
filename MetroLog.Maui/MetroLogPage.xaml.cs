namespace MetroLog.Maui;

public partial class MetroLogPage : ContentPage
{
	private readonly LogController _logController = new();

	public MetroLogPage()
	{
		InitializeComponent();

		NavigationPage.SetHasNavigationBar(this, false);

		Name.Text = AppInfo.Name;
		Version.Text = $"v{AppInfo.VersionString}.{AppInfo.BuildString}";
		Package.Text = AppInfo.PackageName;

		ShareLogs.IsVisible = _logController.CanGetCompressedLogs;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		LogController.SuspendShake();

		DisplayLogs();
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);

		LogController.ResumeShakeIfNeeded();
	}

	private async void DisplayLogs()
	{
		try
		{
			if (_logController.CanGetLogsString)
			{
				var logList = await _logController.GetLogList();
				logList!.Reverse();
				Logs.Text = string.Join(Environment.NewLine, logList);

			}
			else
			{
				Logs.Text = "You need to add a MemoryTarget to the configuration or use AddInMemoryLogger on the maui app builder to retrieved logs as a string...";
			}
		}
		catch (Exception exception)
		{
			Console.WriteLine($"MetroLog.LogController|ERROR while displaying logs: {exception}");
		}
	}

	private async void OnShareLogsClicked(object? sender, EventArgs e)
	{
		try
		{
			var fileName =  $"{Version.Text}_logs.zip";
			var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

			MemoryStream? compressedLogs = await _logController.GetCompressedLogs();

			if (compressedLogs == null)
			{
				return;
			}

			await using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				await compressedLogs.CopyToAsync(fileStream);
			}

			await Share.RequestAsync(new ShareFileRequest
			{
				Title = "Sharing compressed logs",
				File = new ShareFile(filePath)
			});
		}
		catch (Exception exception)
		{
			Console.WriteLine($"MetroLog.LogController|ERROR while sharing compressed logs: {exception}");
		}
	}

	private void OnRefreshClicked(object? sender, EventArgs e)
	{
		DisplayLogs();
	}
}
