using HeartBeat.Models;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HeartBeat.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private IHeartBeatRepository _repo { get; set; }

        public HomeController(IHeartBeatRepository hbr)
        {
            _repo = hbr;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }
    }
}
