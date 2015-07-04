using Microsoft.AspNet.Mvc;
using HeartBeat.Models;

namespace HeartBeat.Controllers
{
    [Route("api/[controller]")]
    public class HeartBeatController : Controller
    {
        private IHeartBeatRepository _repo { get; set;  }

        public HeartBeatController(IHeartBeatRepository hbr)
        {
            _repo = hbr;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_repo.GetHeartBeats());
        }

        [HttpPost]
        public void Post(string group, string device, string service, string status)
        {
            _repo.AddHeartBeat(group, device, service, status);
        }
    }
}
