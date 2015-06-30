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
        private int timeoutSeconds = 15 * 60;

        public IEnumerable<HeartBeatInfo> GetHeartBeats()
        {
            purge(timeoutSeconds);
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
                    if (item.group == group && item.device == device && item.servicce == service)
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
                    HeartBeatInfo hbi = new HeartBeatInfo { group = group, device = device, servicce = service, status = status, time = posted };
                    _info.Add(hbi);
                }
            }
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
