using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public interface IHeartBeatRepository
    {
        IEnumerable<HeartBeatInfo> GetHeartBeats();
        void AddHeartBeat(string group, string device, string service, string status);
    }
}
