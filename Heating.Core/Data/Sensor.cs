using Heating.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Heating.Core
{
    public class Sensor : MyPubSubClient
    {
        public float Reading { get; set; }
        [StringLength(5)]
        public string UOM { get; set; }
        /// <summary>
        /// The amount a value has to change since the last log before it is logged.
        /// </summary>
        public float LogChange { get; set; } = 0.1f;

    }
}
