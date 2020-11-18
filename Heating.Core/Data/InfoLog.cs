using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Heating.Core.Data
{
    public class InfoLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public DateTime LogTime { get; set; }
        public int DeviceId { get; set; }
        //[Column(TypeName = "bit")]
        [StringLength(15)]
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public string Source { get; set; }

        public InfoLog() { }

        public InfoLog(DateTime time, int devId, string devType, string devName, string act, string val, string src)
        {
            LogTime = time;
            DeviceId = devId;
            DeviceType = devType;
            DeviceName = devName;
            Action = act;
            Value = val;
            Source = src;
        }
    }
}
