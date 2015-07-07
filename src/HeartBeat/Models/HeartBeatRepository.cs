using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public class HeartBeatRepository : IHeartBeatRepository
    {
        private static List<HeartBeatInfo> _heartbeats = new List<HeartBeatInfo>();
        private readonly object syncLock = new object();

        public int Timeout { get; set; }

        public HeartBeatRepository()
        {
            Timeout = 15 * 60;  // Default: 15 minutes
        }

        public IEnumerable<HeartBeatInfo> GetAll()
        {
            purge(Timeout);
            return _heartbeats;
        }

        public HeartBeatInfo Get(string Id)
        {
            HeartBeatInfo hbi = null;
            lock (syncLock)
            {
                hbi = PrivateGet(Id);
            }
            return hbi;
        }

        public HeartBeatInfo Add(HeartBeatInfo hbi)
        {
            if (null == hbi || null == hbi.group || null == hbi.device || null == hbi.service || null == hbi.status)
            {
                return null;
            }
            hbi.Id = MakeIdFromInfo(hbi);
            hbi.time = DateTime.Now;
            bool error = false;
            lock (syncLock)
            {
                // Add precludes replacing existing item
                error = (null != PrivateGet(hbi.Id));
                if (!error)
                {
                    _heartbeats.Add(hbi.Copy());
                }
            }
            if (error)
            {
                return null;
            }
            return hbi;
        }

        public void Remove(string Id)
        {
            if (null == Id || Id == "")
            {
                throw new ArgumentOutOfRangeException();
            }
            bool error = false;
            lock (syncLock)
            {
                HeartBeatInfo hbi = PrivateGet(Id);
                if (null == hbi)
                {
                    error = true;
                }
                else
                {
                    _heartbeats.Remove(hbi);
                }
            }
            if(error)
            {
                throw new MissingMemberException();
            }
        }

        public bool Update(HeartBeatInfo hbi)
        {
            HeartBeatInfo found = null;
            lock (syncLock)
            {
                found = PrivateGet(hbi.Id);
                if (null != found)
                {
                    // Only status can change.  Difference in group, device or service is an error
                    if (!(found.group.Equals(hbi.group, StringComparison.CurrentCultureIgnoreCase) &&
                        found.device.Equals(hbi.device, StringComparison.CurrentCultureIgnoreCase) &&
                        found.service.Equals(hbi.service, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        found = null;
                    }
                    else
                    {
                        found.time = DateTime.Now;
                        found.status = hbi.status;
                    }
                }
            }
            return (null != found);
        }

        public void ClearAll()
        {
            purge(0);
        }

        private void purge(int ageInSeconds)
        {
            lock (syncLock)
            {
                long cutoff = DateTime.Now.Ticks - ((long)ageInSeconds * (long)10000000);
                List<HeartBeatInfo> newInfo = new List<HeartBeatInfo>();

                foreach (HeartBeatInfo hbi in _heartbeats)
                {
                    if (hbi.time.Ticks > cutoff)
                        newInfo.Add(hbi);
                }
                _heartbeats = newInfo;
            }
        }

        private string MakeIdFromInfo(HeartBeatInfo hbi)
        {
            StringBuilder id = new StringBuilder(hbi.group);
            id.Append("%09");
            id.Append(hbi.device);
            id.Append("%09");
            id.Append(hbi.service);
            return id.ToString();
        }

        private HeartBeatInfo PrivateGet(string Id)
        {
            HeartBeatInfo hbi = null;
            foreach (HeartBeatInfo aHbi in _heartbeats)
            {
                if (aHbi.Id == Id)
                {
                    hbi = aHbi;
                    break;
                }
            }
            return hbi;
        }
    }
}
