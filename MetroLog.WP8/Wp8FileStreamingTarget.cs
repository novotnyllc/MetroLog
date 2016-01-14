using System;
using MetroLog.Layouts;

namespace MetroLog.Targets
{

    [Obsolete("Use StreamingFileTarget instead")]
    public class Wp8FileStreamingTarget : StreamingFileTarget
    {
        public Wp8FileStreamingTarget()
        {
        }

        public Wp8FileStreamingTarget(Layout layout) : base(layout)
        {
        }
    }
}
