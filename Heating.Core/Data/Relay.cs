using Heating.PubSub;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Heating.Core.Data
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
        public bool IsOn { get; set; } = false;
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


        public async Task SwitchAsync(bool on)
        {
            string pl = ((on) ? OnCommand : OffCommand);
            await MyMqttServer.Publish(Topic, pl);
            IsOn = on;
        }

    }
}
