using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog.WinRT
{
    class EventTarget : SyncTarget
    {
        readonly Action<string> _onMessage;

        public EventTarget(Action<string> onMessage) : base(new SingleLineLayout())
        {
            if (onMessage == null) throw new ArgumentNullException("onMessage");
            _onMessage = onMessage;
        }

        protected override void Write(LogWriteContext context, LogEventInfo entry)
        {
            var message = Layout.GetFormattedString(context, entry);
            _onMessage(message);
        }
    }
}
