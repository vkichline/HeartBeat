using HeartBeat.Controllers;
using HeartBeat.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test
{
    public class HeartBeatControllerTests
    {
        IHeartBeatRepository _hbr;
        HeartBeatController _hbc;

        public HeartBeatControllerTests()
        {
            _hbr = new HeartBeatRepository();
            _hbc = new HeartBeatController(_hbr);
        }

        [Fact]
        public void VerifyControllerExists()
        {
            Assert.NotNull(_hbr);
            Assert.NotNull(_hbc);
        }

        [Fact]
        public void VerifyFirstPostAddsToRepo()
        {
            _hbr.Clear();
            IEnumerable<HeartBeatInfo> beats = _hbr.GetHeartBeats();
            Assert.Equal(0, beats.Count());

            _hbc.Post("TestGroup", "TestDevice", "TestService", "OK");
            beats = _hbr.GetHeartBeats();
            Assert.Equal(1, beats.Count());
        }

        // Verifies that same group, device and service, but different status,
        // simply nupdates status w/o adding new HeartBeatInfo
        [Fact]
        public void VerifyStatusCanBeUpdated()
        {
            _hbr.Clear();
            _hbc.Post("TestGroup", "TestDevice", "TestService", "OK");
            IEnumerable<HeartBeatInfo> beats = _hbr.GetHeartBeats();
            Assert.Equal(1, beats.Count());
            HeartBeatInfo hbi = beats.First();
            Assert.Equal("OK", hbi.status);

            _hbc.Post("TestGroup", "TestDevice", "TestService", "IDLE");
            beats = _hbr.GetHeartBeats();
            Assert.Equal(1, beats.Count());
            hbi = beats.First();
            Assert.Equal("IDLE", hbi.status);
        }

        // Verify that posting different groups, devices, and services add unique HeartBeatInfos
        [Fact]
        public void VerifyUniqueCombinationsAreAdded()
        {
            _hbr.Clear();
            _hbc.Post("A", "a", "1", "Original");
            Assert.Equal(1, _hbr.GetHeartBeats().Count());
            _hbc.Post("A", "a", "2", "Original");
            Assert.Equal(2, _hbr.GetHeartBeats().Count());
            _hbc.Post("A", "b", "1", "Original");
            Assert.Equal(3, _hbr.GetHeartBeats().Count());
            _hbc.Post("A", "b", "2", "Original");
            Assert.Equal(4, _hbr.GetHeartBeats().Count());

            _hbc.Post("B", "a", "1", "Original");
            Assert.Equal(5, _hbr.GetHeartBeats().Count());
            _hbc.Post("B", "a", "2", "Original");
            Assert.Equal(6, _hbr.GetHeartBeats().Count());
            _hbc.Post("B", "b", "1", "Original");
            Assert.Equal(7, _hbr.GetHeartBeats().Count());
            _hbc.Post("B", "b", "2", "Original");
            Assert.Equal(8, _hbr.GetHeartBeats().Count());
        }

        // Verify that all updates take
        [Fact]
        public void VerifyUniqueCombinationsAreUpdated()
        {
            _hbr.Clear();
            _hbc.Post("A", "a", "1", "Original");
            _hbc.Post("A", "a", "2", "Original");
            _hbc.Post("A", "b", "1", "Original");
            _hbc.Post("A", "b", "2", "Original");
            _hbc.Post("B", "a", "1", "Original");
            _hbc.Post("B", "a", "2", "Original");
            _hbc.Post("B", "b", "1", "Original");
            _hbc.Post("B", "b", "2", "Original");
            Assert.Equal(8, _hbr.GetHeartBeats().Count());

            // Update each
            _hbc.Post("A", "a", "1", "Revised");
            _hbc.Post("A", "a", "2", "Revised");
            _hbc.Post("A", "b", "1", "Revised");
            _hbc.Post("A", "b", "2", "Revised");
            _hbc.Post("B", "a", "1", "Revised");
            _hbc.Post("B", "a", "2", "Revised");
            _hbc.Post("B", "b", "1", "Revised");
            _hbc.Post("B", "b", "2", "Revised");
            Assert.Equal(8, _hbr.GetHeartBeats().Count());

            foreach (HeartBeatInfo hbi in _hbr.GetHeartBeats())
            {
                Assert.Equal("Revised", hbi.status);
            }
        }
    }
}
