using Heating.Core.Data;
using Heating.PubSub;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heating.Core
{
    public class SensorCollection
    {
        private List<Sensor> sensors = new List<Sensor>();

        /// <summary>
        /// Construcor
        /// </summary>
        public SensorCollection()
        {
            using (var db = new DataContext())
            {
                db.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Subscribe to all MqttToppics
        /// </summary>
        public void SubscribeToPubSub()
        {
            MyMqttServer.UseMessageReceivedHandler(Handler, "");
        }

        /// <summary>
        /// Handle Mqtt Update
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private int Handler(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic;
            using (var db = new DataContext())
            {
                // look for a Sensor in the database that is subscribed to the received topic
                foreach (var sensor in db.Sensors.ToArray().Where(s => s.Topic == topic))
                {
                    if (arg.ApplicationMessage.Payload.Length > 0)
                    {
                        float val = 0;
                        bool timeUpdate = false;
                        // Get the published value and convert it to a float.
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
                        // If it hasn't updated in the last 30 minutes, forse update now.
                        try
                        {
                            timeUpdate = (DateTime.Now - sensor.LastUpdated) > new TimeSpan(0, 30, 0);
                        }
                        catch
                        {
                            timeUpdate = true; // If there is a problem wth the time, update the falues;
                        }

                        // update if the value changed or time expired.
                        if (val != sensor.Reading || timeUpdate)
                        {
                            sensor.LastUpdated = DateTime.Now;
                            sensor.Reading = val;
                            db.Sensors.Update(sensor);
                            InfoLog lastlog = null;
                            try
                            {
                                lastlog = db.InfoLogs.Last(s => s.DeviceId == sensor.ID && s.DeviceType == "Sensor");
                            } catch { }
                            if (lastlog == null || Math.Abs(float.Parse(lastlog.Value) - sensor.Reading) >= sensor.LogChange)
                            {
                                db.InfoLogs.Add(new InfoLog(DateTime.Now, sensor.ID, "Sensor", sensor.Name, "receive", sensor.Reading.ToString("0.00"), "SensorCollection"));
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            return 0;
        }
    }
}
