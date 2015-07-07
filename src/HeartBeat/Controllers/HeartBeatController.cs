using Microsoft.AspNet.Mvc;
using HeartBeat.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System;

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

        // Get all the HeartBeatInfo records
        [HttpGet]
        IEnumerable<HeartBeatInfo> Get()
        {
            return _repo.GetAll();
        }

        // Get a specific HeartBeatInfo record
        [HttpGet("{id}")]
        IActionResult Get(string Id)
        {
            HeartBeatInfo hbi = _repo.Get(Id);
            if(null == hbi)
            {
                return HttpNotFound();
            }
            return new ObjectResult(hbi);
        }

        [HttpPost]
        public void Post([FromBody] HeartBeatInfo hbi)
        {
            if (null == hbi)
            {
                Context.Response.StatusCode = 400;
            }
            else
            {
                HeartBeatInfo newHbi = _repo.Add(hbi);
                if (null == newHbi)
                {
                    Context.Response.StatusCode = 403;
                    return;
                }
                string url = Url.RouteUrl("Get", new { id = hbi.Id }, Request.Scheme, Request.Host.ToUriComponent());
                Context.Response.StatusCode = 201;
                Context.Response.Headers["Location"] = url;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(string id)
        {
            try
            {
                _repo.Remove(id);
                return new HttpStatusCodeResult(204);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpPut]
        public void Update([FromBody] HeartBeatInfo hbi)
        {
            _repo.Update(hbi);
        }
    }
}
