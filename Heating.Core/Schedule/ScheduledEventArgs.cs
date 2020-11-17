using System;
using System.Collections.Generic;
using System.Text;

namespace Heating.Core.Schedule
{
    public class ScheduledEventArgs : EventArgs
    {
        public int RelayId { get; private set; }
        public bool Action { get; private set; }

        public ScheduledEventArgs(int id, bool action)
        {
            RelayId = id;
            Action = action;
        }
    }
}
