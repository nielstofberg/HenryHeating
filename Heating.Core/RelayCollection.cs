using Heating.Core.Data;
using Heating.Core.PubSub;
using Heating.PubSub;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heating.Core
{
    public class RelayCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RelayCollection()
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
                // look for a Relay in the database that is subscribed to the received topic
                foreach (var relay in db.Relays.ToArray().Where(r => r.Topic == topic))
                {
                    if (arg.ApplicationMessage.Payload.Length > 0)
                    {
                        string payload = Encoding.ASCII.GetString(arg.ApplicationMessage.Payload).ToLower();
                        if (payload == Constant.REFRESH)
                        {
                            db.InfoLogs.Add(new InfoLog(DateTime.Now, relay.ID, "Relay", relay.Name, "refresh", "Refresh Requested from client", "RelayCollection"));
                             var t = relay.SwitchAsync(relay.IsOn);
                        }
                        else
                        {
                            relay.IsOn = (payload == relay.OnCommand);
                            relay.LastUpdated = DateTime.Now;
                            db.Relays.Update(relay);
                            db.InfoLogs.Add(new InfoLog(DateTime.Now, relay.ID, "Relay", relay.Name, "switch", relay.IsOn.ToString(), "RelayCollection"));
                        }
                    }
                }
                db.SaveChanges();
            }
            return 0;
        }
    }
}
