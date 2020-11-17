using Heating.Data;
using Heating.PubSub;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heating.Core
{
    public class Sensor : MyPubSubClient
    {
        public float Reading { get; set; }
        string ReadinigType { get; set; }

        public Sensor(string topic) :
            base(topic)
        {
        }

        protected override int Handler(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ApplicationMessage.Payload.Length > 0)
            {
                float val = 0;
                bool timeUpdate = false;
                if (!float.TryParse(Encoding.ASCII.GetString(arg.ApplicationMessage.Payload), out val))
                {
                    if (arg.ApplicationMessage.Payload.Length == 4)
                    {
                        try
                        {
                            val = BitConverter.ToSingle(arg.ApplicationMessage.Payload, 0);
                            //LastUpdated = DateTime.Now;
                        }
                        catch { }
                    }
                }
                // If it hasn't updated in the last 30 minutes, update now.
                try
                {
                    timeUpdate = (DateTime.Now - LastUpdated) > new TimeSpan(0, 30, 0);
                }
                catch
                {
                    timeUpdate = true; // If there is a problem wth the time, update the falues;
                }

                if (val != Reading || timeUpdate)
                {
                    LastUpdated = DateTime.Now;
                    Reading = val;
                    using (var db = new DataContext())
                    {
                        db.Sensors.Update(this);
                        db.SaveChanges();
                    }
                }

            }
            return 1;
        }

    }
}
