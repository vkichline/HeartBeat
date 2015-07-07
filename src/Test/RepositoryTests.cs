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
        IHeartBeatRepository _hbr = null;

        public RepositoryTests()
        {
            _hbr = new HeartBeatRepository();
        }

        [Fact]
        public void VerifyRepositoryCanBeCreated()
        {
            Assert.NotNull(_hbr);
        }

        [Fact]
        public void VerifyHeartBeatCanBeAdded()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            HeartBeatInfo newHbi = _hbr.Add(hbi);
            Assert.NotNull(newHbi);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
        }

        [Fact]
        public void VerifyHeartBeatCannotBeDuplicated()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            HeartBeatInfo newHbi = _hbr.Add(hbi);
            Assert.NotNull(newHbi);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
            HeartBeatInfo redo = _hbr.Add(hbi);
            Assert.Null(redo);
        }

        [Fact]
        public void VerifyPurge()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            _hbr.Add(hbi);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
            _hbr.Timeout = 0;    // Should cause all to be purged
            beats = _hbr.GetAll();
            Assert.Equal(0, beats.Count());
        }

        [Fact]
        public void VerifyGetAll()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi1 = new HeartBeatInfo { group = "TestGroup1", device = "TestDevice1", service = "TestService1", status = "OK" };
            HeartBeatInfo hbi2 = new HeartBeatInfo { group = "TestGroup2", device = "TestDevice2", service = "TestService2", status = "OK" };
            _hbr.Add(hbi1);
            _hbr.Add(hbi2);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(2, beats.Count());
        }

        [Fact]
        public void VerifyGetById()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi1 = new HeartBeatInfo { group = "TestGroup1", device = "TestDevice1", service = "TestService1", status = "OK" };
            HeartBeatInfo hbi2 = new HeartBeatInfo { group = "TestGroup2", device = "TestDevice2", service = "TestService2", status = "OK" };
            _hbr.Add(hbi1);
            HeartBeatInfo hbiAdded = _hbr.Add(hbi2);
            HeartBeatInfo gotten = _hbr.Get(hbiAdded.Id);
            Assert.True(gotten.group == hbi2.group && gotten.device == hbi2.device && gotten.service == hbi2.service && gotten.status == hbi2.status);
        }

        [Fact]
        public void VerifyRemove()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi1 = new HeartBeatInfo { group = "TestGroup1", device = "TestDevice1", service = "TestService1", status = "OK" };
            HeartBeatInfo hbi2 = new HeartBeatInfo { group = "TestGroup2", device = "TestDevice2", service = "TestService2", status = "OK" };
            _hbr.Add(hbi1);
            HeartBeatInfo hbiAdded = _hbr.Add(hbi2);
            Assert.Equal(2, _hbr.GetAll().Count());
            _hbr.Remove(hbiAdded.Id);
            Assert.Equal(1, _hbr.GetAll().Count());
        }

        [Fact]
        public void VerifyValidUpdate()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            HeartBeatInfo added = _hbr.Add(hbi);
            Assert.Equal(1, _hbr.GetAll().Count());
            added.status = "Changed";
            bool status = _hbr.Update(added);
            Assert.True(status);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
            HeartBeatInfo temp = beats.First();
            Assert.Equal(temp.status, "Changed");
        }

        [Fact]
        public void VerifyInvalidUpdate1()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            HeartBeatInfo added = _hbr.Add(hbi);
            Assert.Equal(1, _hbr.GetAll().Count());
            added.status = "Changed";
            added.group = "Changed";
            bool status = _hbr.Update(added);
            Assert.False(status);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
            HeartBeatInfo temp = beats.First();
            Assert.Equal(temp.status, "OK");
        }

        [Fact]
        public void VerifyInvalidUpdate2()
        {
            _hbr.ClearAll();
            HeartBeatInfo hbi = new HeartBeatInfo { group = "TestGroup", device = "TestDevice", service = "TestService", status = "OK" };
            HeartBeatInfo added = _hbr.Add(hbi);
            Assert.Equal(1, _hbr.GetAll().Count());
            added.Id = "Changed";
            bool status = _hbr.Update(added);
            Assert.False(status);
            IEnumerable<HeartBeatInfo> beats = _hbr.GetAll();
            Assert.Equal(1, beats.Count());
            HeartBeatInfo temp = beats.First();
            Assert.Equal(temp.status, "OK");
        }

    }
}
