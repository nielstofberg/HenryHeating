using Heating.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Heating.Core.Schedule
{
    public class Scheduler
    {
        private Timer _timer;

        public event EventHandler<ScheduledEventArgs> OnScheduledEvent;
        public Scheduler()
        {

        }

        public void Run()
        {
            _timer = new Timer(TimerCallback, null, 1000, 1000);
        }


        public void Stop()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// Runs each time the timer ticks which should be set to every second
        /// </summary>
        /// <param name="state"></param>
        public void TimerCallback(object state)
        {
            using (var dc = new DataContext())
            {
                var now = DateTime.Now;
                var dow = getDays(now);
                var events = dc.ScheduledEvents.Include(r=> r.Relay)
                                                .Where(t => t.Hour == now.Hour 
                                                        && t.Minute == now.Minute 
                                                        && t.LastExe.AddMinutes(1) < now
                                                        && dow.Contains(t.Day))                                                        
                                                .ToArray();
                if (events != null && events.Count() > 0)
                {
                    foreach (var se in events)
                    {
                        OnScheduledEvent?.Invoke(this, new ScheduledEventArgs(se.Relay, se.Action));
                        if (se.Repeat)
                        {
                            se.LastExe = DateTime.Now;
                            dc.ScheduledEvents.Update(se);
                        }
                        else
                        {
                            dc.ScheduledEvents.Remove(se);
                        }
                    }
                    dc.SaveChanges();
                }

                events.Select(t => dow.Contains(t.Day));
            }
        }

        private int[] getDays(DateTime day)
        {
            List<int> ret = new List<int>();
            int d = (int)day.DayOfWeek;
            if (d > 0 && d < 6)
            {
                ret.Add(d);
                ret.Add(8);
                ret.Add(9);
            }
            else
            {
                if(d==0)
                {
                    ret.Add(7);
                }
                else
                {
                    ret.Add(d);
                }
                ret.Add(8);
                ret.Add(10);
            }
            return ret.ToArray();
        }


    }
}