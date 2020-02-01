using System;
using TagLib;
using System.Collections.Generic;

namespace test02
{
    public class TrackResponse
    {
        public int? Disk { get; set; }
        public int TrackNumber { get; set; }
        public string TrackTitle { get; set; }
        public string TrackArtist { get; set; }
        public int Duration { get; set; }
        public string Path { get; set; }
    }
    public class AlbumResponse
    {
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
