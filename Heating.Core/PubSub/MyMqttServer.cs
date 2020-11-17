using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heating.PubSub
{
    public class MyMqttServer
    {
        private IMqttServer _mqttServer;
        private List<MqttSubscription> subscription = new List<MqttSubscription>();

        public MyMqttServer()
        {
            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.StartAsync(new MqttServerOptions());

            _mqttServer.UseApplicationMessageReceivedHandler(MessageHandler);
        }

        private Task MessageHandler(MqttApplicationMessageReceivedEventArgs arg)
        {
            foreach (var ss in subscription.Where(s => s.Topic == arg.ApplicationMessage.Topic))
            {
                ss.Function.Invoke(arg);
            }
            return Task.CompletedTask;
        }

        public async Task Publish(string Topic, byte val)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(new byte[] { val })
                .Build();

            await _mqttServer.PublishAsync(message);
        }

        public async Task Publish(string Topic, string val)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(val)
                .Build();

            await _mqttServer.PublishAsync(message);
        }

        public void UseMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, int> func, string Topic)
        {
            MqttSubscription ss = new MqttSubscription();
            ss.Function = func;
            ss.Topic = Topic;
            subscription.Add(ss);
        }

        public void RemoveMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, int> func)
        {
            var ss = subscription.Where(s => s.Function.Method == func.Method).ToArray();
            if (ss.Length > 0)
            {
                subscription.Remove(ss[0]);
            }
        }

    }
}
