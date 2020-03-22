using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using test02.Models;

namespace test02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackAPIController : ControllerBase
    {
        DataAccessLayer data = new DataAccessLayer();

        [HttpGet]
        [Route("Index")]
        public ActionResult<IEnumerable<TrackResponse>> Index()
        {
            return Ok(data.GetAllTracks().Select(x => new TrackResponse(x)));
        }

        [HttpGet]
        [Route("Get/{id}")]
        public ActionResult<TrackResponse> Get(int id)
        {
            return Ok(new TrackResponse(data.GetTrackById(id)));
        }

        [HttpGet]
        [Route("GetN/{id}/{trackNum}")]
        public ActionResult<TrackResponse> GetN(int id, int trackNum)
        {
            return Ok(new TrackResponse(data.GetTracksFromAlbum(id).Where(x => 
                                        (x.TrackNumber == trackNum)).FirstOrDefault()));
        }

        [HttpPost]
        [Route("Add")]
        public void Add(Tracks track)
        {
            data.AddTrack(track);
        }

        [HttpPut]
        [Route("Update")]
        public void Update(Tracks track)
        {
            data.UpdateTrack(track);
        }
    }
}