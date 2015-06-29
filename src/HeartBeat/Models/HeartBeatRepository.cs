using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public class HeartBeatRepository: IHeartBeatRepository
    {
        private static List<HeartBeatInfo> _info = new List<HeartBeatInfo>();

        public IEnumerable<HeartBeatInfo> GetHeartBeats()
        {
            return _info;
        }

        public bool AddHeartBeat(string group, string device, string service, string status)
        {
            DateTime posted = DateTime.Now;

            foreach(HeartBeatInfo item in _info)
            {
                if(item.group == group && item.device == device && item.servicce == service)
                {
                    item.time = posted;
                    if(item.status == status)
                    {
                        return false;
                    }
                    else
                    {
                        item.status = status;
                        return true;
                    }
                }
            }
            HeartBeatInfo hbi = new HeartBeatInfo { group = group, device = device, servicce = service, status = status, time = posted };
            _info.Add(hbi);
            return true;
        }
    }
}
