using MQTTnet;
using MQTTnet.Server;
using Heating.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Heating.Core.Schedule;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heating.Data;

namespace Heating.Core
{
    /// <summary>
    /// This class represents a single relay. It extends the MyPubSubClient class
    /// which represents a generic MQTT enabled device.
    /// The Relay class switches a relay on or off by publishing a defined values to a
    /// defined topic on the local MQTT server
    /// </summary>
    public class Relay : MyPubSubClient
    {
        [Column(TypeName = "bit")]
        public bool IsOn { get; private set; } = false;
        /// <summary>
        /// The command to be published when an "On" command is applied to the relay
        /// Default is "1"
        /// </summary>
        public string OnCommand { get; set; } = "1";

        /// <summary>
        /// The command to be published when an "Off" command is applied to the relay
        /// Default is "0"
        /// </summary>
        public string OffCommand { get; set; } = "0";

        public Relay() { }

        public Relay(string topic) :
            base(topic)
        {
        }

        protected override int Handler(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ApplicationMessage.Payload.Length > 0)
            {
                IsOn = Encoding.ASCII.GetString(arg.ApplicationMessage.Payload) == OnCommand;
                LastUpdated = DateTime.Now;
                using (var db = new DataContext())
                {
                    db.Relays.Update(this);
                    db.SaveChanges();
                }
            }
            return 1;
        }

        public async Task Switch(bool on)
        {
            string pl = ((on) ? OnCommand : OffCommand);
            await _broker.Publish(Topic, pl);
            IsOn = on;
        }
    }
}
