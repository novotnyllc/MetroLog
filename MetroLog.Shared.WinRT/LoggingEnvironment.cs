using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.System;
using Windows.UI.Xaml;
using Windows.Storage;

namespace MetroLog
{
    /// <summary>
    /// Holds the logging environment for Windows Store apps.
    /// </summary>
    public class LoggingEnvironment : LoggingEnvironmentBase
    {
        public string PackageArchitecture { get; private set; }
        public string PackageFullName { get; private set; }
        public string PackagePublisher { get; private set; }
        public string PackagePublisherId { get; private set; }
        public string PackageResourceId { get; private set; }
        public string PackageVersion { get; private set; }

        /// <summary>
        /// Gets a unique ID for the installation of this app.
        /// </summary>
        public string InstallationId { get; private set; }

        /// <summary>
        /// Holds whether we are able to access a running XAML application.
        /// </summary>
        static XamlApplicationState _xamlApplicationState; 

        public LoggingEnvironment()
#if WINDOWS_PHONE_APP
        : base("Windows Phone App 8.1")
#else
            : base(".NET Core")
#endif
        {
            // id...
            var id = Package.Current.Id;
            this.PackageArchitecture = id.Architecture.ToString();
            this.PackageFullName = id.FullName;
            this.PackagePublisher = id.Publisher;
            this.PackagePublisherId = id.PublisherId;
            this.PackageResourceId = id.ResourceId;
            this.PackageVersion = $"{id.Version.Major}.{id.Version.Minor}.{id.Version.Build}.{id.Version.Revision}";

            // installation...
            const string containerName = "MetroLog";
            var ls = ApplicationData.Current.LocalSettings;
            if(!(ls.Containers.ContainsKey(containerName)))
                ls.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);

            // then..
            const string key = "InstallationId";
            this.InstallationId = (string)ls.Values[key];
            if (string.IsNullOrEmpty(this.InstallationId))
            {
                this.InstallationId = Guid.NewGuid().ToString();
                ls.Values[key] = this.InstallationId;
            }
        }

        internal static XamlApplicationState XamlApplicationState
        {
            get
            {
                if (_xamlApplicationState == MetroLog.XamlApplicationState.Unknown)
                {
                    if (DesignMode.DesignModeEnabled)
                    {
                        _xamlApplicationState = XamlApplicationState.Unavailable;
                    }
                    else
                    {
                        // the expected behaviour here is the exception - the else case is provided to
                        // ensure the compiler doesn't optimize out the check (and to be thorough)...
                        try
                        {
                            if (Application.Current != null)
                                _xamlApplicationState = XamlApplicationState.Available;
                            else
                                _xamlApplicationState = XamlApplicationState.Unavailable;
                        }
                        catch
                        {
                            _xamlApplicationState = XamlApplicationState.Unavailable;
                        }
                    }
                    
                }
                return _xamlApplicationState;
            }
        }
    }
}
