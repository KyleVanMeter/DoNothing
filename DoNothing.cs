using System;
using TagLib;
using System.Collections.Generic;
using System.Linq;

using test02.Models;


namespace test02
{
    public class TrackResponse
    {
        public TrackResponse() { }
        public TrackResponse(Tracks tracks)
        {
            Disk = tracks.Disk;
            TrackNumber = tracks.TrackNumber;
            TrackTitle = tracks.TrackTitle;
            TrackArtist = tracks.TrackArtist;
            Duration = tracks.Duration;
            Path = tracks.Path;
        }
        public int? Disk { get; set; }
        public int TrackNumber { get; set; }
        public string TrackTitle { get; set; }
        public string TrackArtist { get; set; }
        public string Duration { get; set; }
        public string Path { get; set; }
    }
    public class AlbumResponse
    {
        public AlbumResponse() { }
        public AlbumResponse(Album album)
        {
            AlbumName = album.AlbumTitle;
            Artists = album.AlbumArtist;
            Year = album.Year;
            Tracks = album.Tracks.Select(x => new TrackResponse(x)).ToList();
        }
        public string AlbumName { get; set; }
        public string Artists { get; set; }
        public int Year { get; set; }
        public List<TrackResponse> Tracks { get; set; }
    }

    public class DoNothing
    {
        public int Stuff { get; set; }
        public string Album { get; set; }
        public string[] Artist { get; set; }
        public string Year { get; set; }
        public string Time { get; set; }
        public string Track { get; set; }
    }
}
