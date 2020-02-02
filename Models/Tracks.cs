using System;
using System.Collections.Generic;

namespace test02.Models
{
    public partial class Tracks
    {
        public int Id { get; set; }
        public int? AlbumId { get; set; }
        public int? Disk { get; set; }
        public int TrackNumber { get; set; }
        public string TrackTitle { get; set; }
        public string TrackArtist { get; set; }
        public string Duration { get; set; }
        public string Path { get; set; }

        public virtual Album Album { get; set; }
    }
}
