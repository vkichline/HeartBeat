using Microsoft.AspNet.Mvc;
using HeartBeat.Models;

namespace HeartBeat.Controllers
{
    [Route("api/[controller]")]
    public class HeartBeatController : Controller
    {
        private HeartBeatRepository _repo = new HeartBeatRepository();

        public HeartBeatController()
        {
            _repo.AddHeartBeat("Kichline.Kirkland", "RPi2", "Test", "OK");
            _repo.AddHeartBeat("Kichline.Kirkland", "RPi1", "Test", "OK");
            _repo.AddHeartBeat("Kichline.Kirkland", "RPi2", "Cam", "OK");
            _repo.AddHeartBeat("Kichline.Kirkland", "RPi2", "Cam", "OK");
            _repo.AddHeartBeat("Kichline.Kirkland", "RPi2", "Cam", "IDLE");
        }

        // GET: api/HeartBeat/

        [HttpGet]
        public IActionResult Index()
        {
            return View(_repo.GetHeartBeats());
        }

        // POST: api/HeartBeat/
        [HttpPost]
        public void Post(string group, string device, string service, string status)
        {
            _repo.AddHeartBeat(group, device, service, status);
        }
    }
}
