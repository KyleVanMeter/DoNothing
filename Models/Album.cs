using System;
using System.Collections.Generic;

namespace test02.Models
{
    public partial class Album
    {
        public Album()
        {
            Tracks = new HashSet<Tracks>();
        }

        public int Id { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public int Year { get; set; }
        public string AlbumArtPath { get; set; }

        public virtual ICollection<Tracks> Tracks { get; set; }
    }
}
