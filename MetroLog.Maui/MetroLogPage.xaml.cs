namespace MetroLog.Maui;

public partial class MetroLogPage : ContentPage
{
	private readonly LogController _logController = new();

	public MetroLogPage()
	{
		InitializeComponent();

		NavigationPage.SetHasNavigationBar(this, false);

		if (Application.Current!.Resources.TryGetValue(LogPageResources.LogPageFontFamily, out object fontFamily)
		    && fontFamily is string stringFontFamily)
		{
			Name.FontFamily = Version.FontFamily = Package.FontFamily =
				Logs.FontFamily = ShareLogs.FontFamily = Refresh.FontFamily = stringFontFamily;
		}

		if (Application.Current.Resources.TryGetValue(LogPageResources.LogPageTextColor, out object textColor)
		    && textColor is Color colorTextColor)
		{
			Title.TextColor = Name.TextColor = Version.TextColor = Package.TextColor =
				Logs.TextColor = ShareLogs.TextColor = Refresh.TextColor = colorTextColor;
		}

		if (Application.Current.Resources.TryGetValue(LogPageResources.LogPageButtonColor, out object buttonColor)
		    && buttonColor is Color colorButtonColor)
		{
			Close.BackgroundColor = Refresh.BackgroundColor = ShareLogs.BackgroundColor = colorButtonColor;
		}

		if (Application.Current.Resources.TryGetValue(LogPageResources.LogPageBackgroundColor, out object backgroundColor)
		    && backgroundColor is Color colorBackgroundColor)
		{
			BackgroundColor = colorBackgroundColor;
		}

		if (Application.Current.Resources.TryGetValue(LogPageResources.LogPageLogsBackgroundColor, out object logsBackgroundColor)
		    && logsBackgroundColor is Color colorLogsBackgroundColor)
		{
			LogsBorder.BackgroundColor = colorLogsBackgroundColor;
		}

		if (Application.Current.Resources.TryGetValue(LogPageResources.LogPageLogsBorderColor, out object logsBorderColor)
		    && logsBorderColor is Color colorLogsBorderColor)
		{
			LogsBorder.Stroke = colorLogsBorderColor;
		}

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

	private async void OnCloseClicked(object? sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}
