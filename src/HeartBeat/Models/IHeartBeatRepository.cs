using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public interface IHeartBeatRepository
    {
        int Timeout { get; set; }
        IEnumerable<HeartBeatInfo> GetHeartBeats();
        void AddHeartBeat(string group, string device, string service, string status);
    }
}
