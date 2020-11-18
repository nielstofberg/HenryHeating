using Heating.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heating.Core.Schedule
{
    public class ScheduledEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        [Column(TypeName = "bit")]
        public bool Action { get; set; }
        [Column(TypeName = "bit")]
        public bool Repeat { get; set; }
        public int RelayID { get; set; }
        public DateTime LastExe { get; set; }
        public virtual Relay Relay { get; set; }
    }
}
