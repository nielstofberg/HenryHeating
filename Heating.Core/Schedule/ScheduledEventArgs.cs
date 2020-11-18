using Heating.Core.Data;
using System;

namespace Heating.Core.Schedule
{
    public class ScheduledEventArgs : EventArgs
    {
        public Relay Relay { get; private set; }
        public bool Action { get; private set; }

        public ScheduledEventArgs(Relay relay, bool action)
        {
            Action = action;
            Relay = relay;
        }
    }
}
