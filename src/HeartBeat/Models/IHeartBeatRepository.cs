using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeartBeat.Models
{
    public interface IHeartBeatRepository
    {
        int Timeout { get; set; }

        IEnumerable<HeartBeatInfo> GetAll();

        HeartBeatInfo Get(string Id);

        HeartBeatInfo Add(HeartBeatInfo hbi);

        void Remove(string Id);

        bool Update(HeartBeatInfo hbi);

        void ClearAll();
    }
}
