using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public interface IHeartBeatRepository
    {
        IEnumerable<HeartBeatInfo> GetHeartBeats();
        // returns true if added, false if dupe
        bool AddHeartBeat(string group, string device, string service, string status);
    }
}
