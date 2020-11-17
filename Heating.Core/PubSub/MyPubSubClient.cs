using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Heating.PubSub
{
    /// <summary>
    /// This Class represents a generic MQTT enabled device and should be
    /// inherrited to represent something more specific.
    /// </summary>
    public class MyPubSubClient
    {
        public static readonly string REFRESH_TOPIC = "REFRESH";
        protected static MyMqttServer _broker = null;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string DeviceName { get; set; }
        public DateTime? LastUpdated { get; set; }

        public MyPubSubClient()
        {
            if (_broker == null)
            {
                _broker = new MyMqttServer();
            }
        }

        public MyPubSubClient(string topic)
        {
            Topic = topic;
            Subscribe();
        }

        public void Subscribe()
        {            
            _broker.UseMessageReceivedHandler(Handler, Topic);
        }

        protected virtual int Handler(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ApplicationMessage.Payload.Length > 0)
            {
                //IsOn = arg.ApplicationMessage.Payload[0] == 0x31;
            }
            return 1;
        }

        public void Refresh()
        {
            var t = _broker.Publish(REFRESH_TOPIC, "refresh");
        }
    }
}
