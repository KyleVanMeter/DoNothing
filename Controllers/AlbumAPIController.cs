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
    public class AlbumAPIController : ControllerBase
    {
        DataAccessLayer data = new DataAccessLayer();

        [HttpGet]
        [Route("Index")]
        public ActionResult<IEnumerable<AlbumResponse>> Index()
        {
            return Ok(data.GetAllAlbums().Select(x => new AlbumResponse(x)).ToArray());
        }

        [HttpGet]
        [Route("Get/{id}")]
        public ActionResult<AlbumResponse> Get(int id)
        {
            try
            {
                return Ok(new AlbumResponse(data.GetAlbumById(id)));
            } catch (Exception ex)
            {
                Console.WriteLine("Exception at AlbumAPI Get with: " + ex.Message);
                return new StatusCodeResult(404);
            }
        }

        [HttpGet]
        [Route("Tracks/{id}")]
        public ActionResult<List<TrackResponse>> GetTracks(int id)
        {
            return Ok(data.GetTracksFromAlbum(id).Select(x => new TrackResponse(x)).ToList());
        }

        [HttpPost]
        [Route("Add")]
        public void Add(Album album)
        {
            data.AddAlbum(album);
        }

        [HttpPut]
        [Route("Update")]
        public void Update(Album album)
        {
            data.UpdateAlbum(album);
        }
    }
}