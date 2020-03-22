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
            try
            {
                return Ok(new TrackResponse(data.GetTrackById(id)));
            } catch (Exception ex)
            {
                Console.WriteLine("Exception at TrackAPI Get with: " + ex.Message);
                return new StatusCodeResult(404);
            }
        }

        [HttpGet]
        [Route("GetN/{id}/{trackNum}")]
        public ActionResult<TrackResponse> GetN(int id, int trackNum)
        {
            try
            {
                return Ok(new TrackResponse(data.GetTracksFromAlbum(id).Where(x =>
                                        (x.TrackNumber == trackNum)).FirstOrDefault()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at TrackAPI GetN with: " + ex.Message);
                return new StatusCodeResult(404);
            }
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