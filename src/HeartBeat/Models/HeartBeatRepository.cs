using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public class HeartBeatRepository: IHeartBeatRepository
    {
        private static List<HeartBeatInfo> _info = new List<HeartBeatInfo>();
        private readonly object syncLock = new object();

        public int Timeout { get; set; }

        public HeartBeatRepository()
        {
            Timeout = 15 * 60;  // Default: 15 minutes
        }

        public IEnumerable<HeartBeatInfo> GetHeartBeats()
        {
            purge(Timeout);
            return _info;
        }

        public void AddHeartBeat(string group, string device, string service, string status)
        {
            DateTime posted = DateTime.Now;
            bool found = false;

            lock(syncLock)
            {
                foreach (HeartBeatInfo item in _info)
                {
                    if (item.group == group && item.device == device && item.service == service)
                    {
                        item.time = posted;
                        if (item.status == status)
                        {
                            found = true;
                            break;
                        }
                        else
                        {
                            found = true;
                            item.status = status;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    HeartBeatInfo hbi = new HeartBeatInfo { group = group, device = device, service = service, status = status, time = posted };
                    _info.Add(hbi);
                }
            }
        }

        public void Clear()
        {
            purge(0);
        }

        private void purge(int ageInSeconds)
        {
            lock (syncLock)
            {
                long cutoff = DateTime.Now.Ticks - ((long)ageInSeconds * (long)10000000);
                List<HeartBeatInfo> newInfo = new List<HeartBeatInfo>();

                foreach (HeartBeatInfo hbi in _info)
                {
                    if (hbi.time.Ticks > cutoff)
                        newInfo.Add(hbi);
                }
                _info = newInfo;
            }
        }
    }
}
