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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetroLog
{
    /// <summary>
    /// Holds the logging environment for Windows Store apps.
    /// </summary>
    public partial class LoggingEnvironment : LoggingEnvironmentBase
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ProcessorArchitecture PackageArchitecture { get; private set; }

        public string PackageFullName { get; private set; }
        public string PackagePublisher { get; private set; }
        public string PackagePublisherId { get; private set; }
        public string PackageResourceId { get; private set; }
        public string PackageVersion { get; private set; }

        public LoggingEnvironment()
            : base(".NET Core")
        {
            // id...
            var id = Package.Current.Id;
            this.PackageArchitecture = id.Architecture;
            this.PackageFullName = id.FullName;
            this.PackagePublisher = id.Publisher;
            this.PackagePublisherId = id.PublisherId;
            this.PackageResourceId = id.ResourceId;
            this.PackageVersion = string.Format("{0}.{1}.{2}.{3}", id.Version.Major, id.Version.Minor,
                id.Version.Build, id.Version.Revision);
        }
    }
}
