using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public class HeartBeatInfo
    {
        public string group { get; set; }
        public string device { get; set; }
        public string servicce { get; set; }
        public string status { get; set; }
        public DateTime time { get; set; }
    }
}
