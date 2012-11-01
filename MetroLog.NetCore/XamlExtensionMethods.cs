extern alias pcl;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MetroLog
{
    /// <summary>
    /// Extension methods for adding logging to XAML types.
    /// </summary>
    public static class XamlExtensionMethods
    {
        /// <summary>
        /// Gets a logger for the given user control instance.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="config">Optional configuration.</param>
        /// <returns></returns>
        public static ILogger GetLogger(this UserControl control, LoggingConfiguration config = null)
        {
            return pcl::MetroLog.LogManagerFactory.DefaultLogManager.GetLogger(control.GetType(), config);
        }
    }
}
