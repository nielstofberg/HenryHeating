using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heating.PubSub
{
    public struct MqttSubscription
    {
        public string Topic { get; set;}
        public Func<MqttApplicationMessageReceivedEventArgs, int> Function { get; set; }
    }
}
