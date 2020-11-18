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
        private static IMqttServer _mqttServer;
        private static List<MqttSubscription> subscription = new List<MqttSubscription>();

        static MyMqttServer()
        {
            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.StartAsync(new MqttServerOptions());

            _mqttServer.UseApplicationMessageReceivedHandler(MessageHandler);
        }

        private static Task MessageHandler(MqttApplicationMessageReceivedEventArgs arg)
        {
            // Call the handler functions for all subscriptions to the received topic or blank subscriptions.
            foreach (var ss in subscription.Where(s => (s.Topic == arg.ApplicationMessage.Topic) || (s.Topic.Length==0)))
            {
                ss.Function.Invoke(arg);
            }
            return Task.CompletedTask;
        }

        public static async Task Publish(string Topic, byte val)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(new byte[] { val })
                .Build();

            await _mqttServer.PublishAsync(message);
        }

        public static async Task Publish(string Topic, string val)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(val)
                .Build();

            await _mqttServer.PublishAsync(message);
        }

        /// <summary>
        /// Subscribe to the a topic. The handler function will be called whenever the specified topic has been published to.
        /// If the topic is omitted, the handler will be called for all published posts.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="Topic">If topic is blank, all posts will be passed to the handler</param>
        public static void UseMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, int> func, string Topic="")
        {
            MqttSubscription ss = new MqttSubscription();
            ss.Function = func;
            ss.Topic = Topic;
            subscription.Add(ss);
        }

        public static void RemoveMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, int> func)
        {
            var ss = subscription.Where(s => s.Function.Method == func.Method).ToArray();
            if (ss.Length > 0)
            {
                subscription.Remove(ss[0]);
            }
        }

    }
}
