using HeartBeat.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class RepositoryTests
    {
        public RepositoryTests()
        {
        }

        [Fact]
        public void VerifyRepositoryCanBeCreated()
        {
            IHeartBeatRepository hbr = new HeartBeatRepository();
            Assert.NotNull(hbr);
        }

        [Fact]
        public void VerifyHeartBeatCanBeAdded()
        {
            IHeartBeatRepository hbr = new HeartBeatRepository();
            hbr.AddHeartBeat("TestGroup", "TestDevice", "TestService", "OK");
            IEnumerable<HeartBeatInfo> beats = hbr.GetHeartBeats();
            Assert.Equal(1, beats.Count());
        }

        [Fact]
        public void VerifyPurge()
        {
            IHeartBeatRepository hbr = new HeartBeatRepository();
            hbr.AddHeartBeat("TestGroup", "TestDevice", "TestService", "OK");
            IEnumerable<HeartBeatInfo> beats = hbr.GetHeartBeats();
            Assert.Equal(1, beats.Count());
            hbr.Timeout = 0;    // Should cause all to be purged
            beats = hbr.GetHeartBeats();
            Assert.Equal(0, beats.Count());
        }
    }
}
