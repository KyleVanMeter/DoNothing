﻿using System;
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

        [HttpGet]
        [Route("api/Album/Index")]
        IEnumerable<Album> IAlbum.Index()
        {
            return data.GetAllAlbums();
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
        public IEnumerable<DoNothing> Get()
        {
            string folder = @"E:\Music\Main\Bleep\";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var queryFile = from file in fileList select file;
            var queryMP3 = from file in queryFile 
                           where file.Extension == ".mp3" 
                           orderby file.Name select file;
            var queryFLAC = from file in queryFile 
                            where file.Extension == ".flac" 
                            orderby file.Name select file;

            List<DoNothing> container = new List<DoNothing>
            {
                new DoNothing { }
            };

            DataAccessLayer data = new DataAccessLayer();
            data.AddFolder(@"E:\Music\Main\Rap\");

            foreach(Album album in data.GetAllAlbums())
            {
                Console.WriteLine(album.AlbumTitle);
                container.Add(new DoNothing
                {
                    Track = album.Tracks.Count.ToString(),
                    Album = album.AlbumTitle,
                    Artist = new String[] { album.AlbumArtist },
                    Year = album.Year.ToString(),
                    Time = "?"
                });

                foreach(Tracks track in data.GetTracksFromAlbum(album.Id))
                {
                    container.Add(new DoNothing
                    {
                        Track = track.Disk.ToString() + '.' + track.TrackNumber.ToString(),
                        Album = track.TrackTitle,
                        Artist = new String[] { track.TrackArtist },
                        Year = track.Album.Year.ToString(),
                        Time = track.Duration.ToString()
                    });
                }
            }

            container.RemoveAll(x => x.Artist == null);
            container = container.OrderBy(x => x.Artist.First()).ToList();

            return container.ToArray();
        }
    }
}