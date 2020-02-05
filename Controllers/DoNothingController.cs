using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test02.Models;

using TagLib;

namespace test02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoNothingController : ControllerBase
    {
        DataAccessLayer data = new DataAccessLayer();

        [HttpGet]
        public IEnumerable<AlbumResponse> Get()
        {
            List<AlbumResponse> AlbumAgg = new List<AlbumResponse>();
            DataAccessLayer data = new DataAccessLayer();
            //data.AddFolder(@"E:\Music\Main\Rap\");

            foreach(Album album in data.GetAllAlbums())
            {
                List<Tracks> tracks = data.GetTracksFromAlbum(album.Id);
                List<TrackResponse> trackResponses = new List<TrackResponse>();

                foreach(Tracks track in tracks)
                {
                    trackResponses.Add(new TrackResponse
                    {
                        Disk = track.Disk,
                        TrackNumber = track.TrackNumber,
                        TrackArtist = track.TrackArtist,
                        TrackTitle = track.TrackTitle,
                        Duration = track.Duration,
                        Path = track.Path
                    });
                }

                AlbumAgg.Add(new AlbumResponse
                {
                    AlbumName = album.AlbumTitle,
                    Artists = album.AlbumArtist,
                    Year = album.Year,
                    Tracks = trackResponses
                });
            }

            AlbumAgg = AlbumAgg.OrderBy(x => x.Artists.First()).ToList();

            return AlbumAgg.ToArray();
        }
    }
}