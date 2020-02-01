using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test02.Models;

using TagLib;

namespace test02.Controllers
{
    interface IAlbum
    {
        IEnumerable<Album> Index();
        Album Get(int id);
        List<Tracks> GetTracks(int id);
        void Add(Album album);
        void Update(Album album);
    }
    interface ITracks
    {
        IEnumerable<Tracks> Index();
        Tracks Get(int id);
        void Add(Tracks track);
        void Update(Tracks track);
        
    }

    [ApiController]
    [Route("[controller]")]
    public class DoNothingController : ControllerBase, IAlbum, ITracks
    {
        DataAccessLayer data = new DataAccessLayer();

        [HttpGet("please")]
        int test()
        {
            return 42;
        }

        [HttpGet]
        [Route("/test")]
        IEnumerable<Album> IAlbum.Index()
        {
            Console.WriteLine("In IAlbum Index");
            return data.GetAllAlbums().ToArray();
        }

        [HttpGet]
        [Route("api/Album/Get/{id}")]
        Album IAlbum.Get(int id)
        {
            return data.GetAlbumById(id);
        }

        [HttpGet]
        [Route("api/Album/Tracks/{id}")]
        List<Tracks> IAlbum.GetTracks(int id)
        {
            return data.GetTracksFromAlbum(id);
        }

        [HttpPost]
        [Route("api/Album/Add")]
        void IAlbum.Add(Album album)
        {
            data.AddAlbum(album);
        }

        [HttpPut]
        [Route("api/Album/Update")]
        void IAlbum.Update(Album album)
        {
            data.UpdateAlbum(album);
        }

        [HttpGet]
        [Route("api/Tracks/Index")]
        IEnumerable<Tracks> ITracks.Index()
        {
            return data.GetAllTracks();
        }

        [HttpGet]
        [Route("api/Tracks/Get/{id}")]
        Tracks ITracks.Get(int id)
        {
            return data.GetTrackById(id);
        }

        [HttpPost]
        [Route("api/Tracks/Add")]
        void ITracks.Add(Tracks track)
        {
            data.AddTrack(track);
        }

        [HttpPut]
        [Route("api/Tracks/Update")]
        void ITracks.Update(Tracks track)
        {
            data.UpdateTrack(track);
        }

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
                        Path = "N/A"
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