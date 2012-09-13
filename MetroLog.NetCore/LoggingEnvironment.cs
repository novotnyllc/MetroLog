using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.System;
using Windows.UI.Xaml;

namespace MetroLog
{
    public partial class LoggingEnvironment : LoggingEnvironmentBase
    {
        public LoggingEnvironment()
            : base(".NET Core")
        {
            // id...
            var id = Package.Current.Id;
            this.Values["PackageArchitecture"] = id.Architecture;
            this.Values["PackageFamilyName"] = id.FamilyName;
            this.Values["PackageFullName"] = id.FullName;
            this.Values["PackageName"] = id.Name;
            this.Values["PackagePublisher"] = id.Publisher;
            this.Values["PackagePublisherId"] = id.PublisherId;
            this.Values["PackageResourceId"] = id.ResourceId;
            this.Values["PackageVersion"] = id.Version;
        }

        public override string ToJson()
        {
            var obj = new JsonObject();
            foreach (string key in this.Values.Keys)
            {
                object value = this.Values[key];
                if (value != null)
                    value = value.ToString();
                else
                    value = string.Empty;

                // add...
                obj.Add(key, JsonValue.CreateStringValue((string)value));
            }

            // ok...
            return obj.Stringify();
        }
    }
}
