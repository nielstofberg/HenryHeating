using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heating.Core.Data
{
    /// <summary>
    /// This Class represents a generic MQTT enabled device and should be
    /// inherrited to represent something more specific.
    /// </summary>
    public class MyPubSubClient
    {
        public static readonly string REFRESH_TOPIC = "REFRESH";

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
        }


    }
}
