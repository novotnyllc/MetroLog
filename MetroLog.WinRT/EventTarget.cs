using System;

using Guards;

using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog.WinRT
{
    class EventTarget : SyncTarget
    {
        private readonly Action<string> _onMessage;

        public EventTarget(Action<string> onMessage) : base(new SingleLineLayout())
        {
            Guard.ArgumentNotNull(() => onMessage);

            _onMessage = onMessage;
        }

        protected override void Write(LogWriteContext context, LogEventInfo entry)
        {
            var message = Layout.GetFormattedString(context, entry);
            _onMessage(message);
        }
    }
}
