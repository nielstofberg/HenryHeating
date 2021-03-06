﻿using System;
using System.Linq;
using System.Collections.Generic;
using Heating.PubSub;
using System.IO;
using Heating.Core.Schedule;
using Microsoft.Extensions.Configuration;

namespace Heating.Core
{
    public class HeatingManager
    {
        private Scheduler _scheduler;

        public RelayCollection Relays { get; private set;}
        public SensorCollection Sensors { get; private set;}

        static void Main()
        {
            var HeatingManager = new HeatingManager();
        }

        public HeatingManager()
        {
            Sensors = new SensorCollection();
            Relays = new RelayCollection();
            Relays.SubscribeToPubSub();
            Sensors.SubscribeToPubSub();
            _scheduler = new Scheduler();
            _scheduler.OnScheduledEvent += _scheduler_OnScheduledEvent;
            _scheduler.Run();
        }

        private async void _scheduler_OnScheduledEvent(object sender, ScheduledEventArgs e)
        {
            await e.Relay.SwitchAsync(e.Action);
        }
    }
}
