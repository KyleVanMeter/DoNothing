using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

using SixLabors.ImageSharp;
using TagLib;

namespace test02.Models
{
    public class DataAccessLayer
    {
        ThingContext db = new ThingContext();

        private string ConvertTime(TimeSpan time)
        {
            string result;
            string sec = time.Seconds <= 9 ? "0" + time.Seconds : time.Seconds.ToString();

            if(time.Hours == 0)
            {
                result = time.Minutes + ":" + sec;
            } else
            {
                result = time.Hours + ":" + time.Minutes + ":" + sec;
            }

            return result;
        }
        public string GetAlbumArt(string Folder)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Folder);
            if (dir.Exists)
            {
                string[] extensions = new string[] { ".jpg", ".jpeg", ".png", ".tiff", ".gif", ".bmp"};
                IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                var queryFile = from file in fileList select file;
                var queryExt = from file in queryFile
                               where extensions.Contains(file.Extension) 
                               && Path.GetFileNameWithoutExtension(file.FullName).ToLower() == "cover"
                               orderby file.Length
                               select file;

                if (queryExt.Any())
                {
                    return queryExt.First().FullName;
                }
            }

            return null;
        }
        public string GetEmbedAlbumArt(TagLib.File file, string Folder)
        {
            string path = new FileInfo(Folder).Directory.Name;
            path += "cover.png";
            TagLib.IPicture[] pictures = file.Tag.Pictures;
            if (pictures.Any())
            {
                throw new NotImplementedException();
                /*TagLib.IPicture picture = pictures.First();
                Image image = Image.Load(picture.Data.Data);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                using (FileStream fs = System.IO.File.Create(path))
                {
                    image.SaveAsPng(fs);
                }

                return path;*/
            }

            return null;
        }
        public void AddFolder(string Folder)
        {
            string[] extensions = new string[] { ".mp3", "flac", ".m4a", ".ape", ".wav", ".ogg", ".alac", ".aiff", ".aac" };

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Folder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var queryFile = from file in fileList select file;
            var queryExt = from file in queryFile
                           where extensions.Contains(file.Extension)
                           orderby file.Name
                           select file;
            string temp = "";
            List<Tracks> tempTrackList = new List<Tracks>();
            foreach (System.IO.FileInfo fi in queryExt)
            {
                Console.WriteLine("At {0}", fi.FullName);
                var tFile = TagLib.File.Create(fi.FullName);
                var cond = db.Album.Where(t => t.AlbumTitle.Contains(tFile.Tag.Album));
                if (!cond.Any())
                {
                    Console.WriteLine("Updating Album to {0}, from {1}", tFile.Tag.Album, temp);
                    temp = tFile.Tag.Album;
                    
                    AddAlbum(new Album
                    {
                        AlbumArtist = tFile.Tag.JoinedAlbumArtists,
                        AlbumTitle = tFile.Tag.Album,
                        Id = GetGreatestAlbumId() + 1,
                        Year = (int)tFile.Tag.Year,
                        Tracks = new List<Tracks>(),
                        AlbumArtPath = GetAlbumArt(dir.FullName) ?? GetEmbedAlbumArt(tFile, dir.FullName)
                    });

                    AddTrack(new Tracks
                    {
                        Id = GetGreatestTrackId() + 1,
                        AlbumId = GetGreatestAlbumId(),
                        Disk = (int)tFile.Tag.Disc,
                        TrackNumber = (int)tFile.Tag.Track,
                        TrackTitle = tFile.Tag.Title,
                        TrackArtist = tFile.Tag.JoinedPerformers,
                        Duration = ConvertTime(tFile.Properties.Duration),
                        Path = fi.FullName
                    });
                } else
                {
                    int albumId = cond.Select(t => t.Id).FirstOrDefault();
                    AddTrack(new Tracks
                    {
                        Id = GetGreatestTrackId() + 1,
                        AlbumId = albumId,
                        Disk = (int)tFile.Tag.Disc,
                        TrackNumber = (int)tFile.Tag.Track,
                        TrackTitle = tFile.Tag.Title,
                        TrackArtist = tFile.Tag.JoinedPerformers,
                        Duration = ConvertTime(tFile.Properties.Duration),
                        Path = fi.FullName
                    });
                }
            }
        }
        public IEnumerable<Album> GetAllAlbums()
        {
            try
            {
                return db.Album.ToList();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Tracks> GetAllTracks()
        {
            try
            {
                return db.Tracks.ToList();
            } 
            catch
            {
                throw;
            }
        }

        public void AddAlbum(Album album)
        {
            try
            {
                db.Album.Add(album);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void AddTrack(Tracks track)
        {
            try
            {
                db.Tracks.Add(track);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateAlbum(Album album)
        {
            try
            {
                db.Entry(album).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTrack(Tracks track)
        {
            try
            {
                db.Entry(track).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Album GetAlbumById(int id)
        {
            try
            {
                Album album = db.Album.Find(id);
                return album;
            }
            catch
            {
                throw;
            }
        }

        public Tracks GetTrackById(int id)
        {
            try
            {
                Tracks track = db.Tracks.Find(id);
                return track;
            }
            catch
            {
                throw;
            }
        }

        public List<Tracks> GetTracksFromAlbum(int id)
        {

            List<Tracks> trackListing = new List<Tracks>();
            trackListing = (from Track in db.Tracks 
                            where Track.AlbumId == id select Track).ToList();

            return trackListing;
        }

        public int GetGreatestAlbumId()
        {
            int? maxId = db.Album.Max(u => (int?)u.Id);

            if(maxId.HasValue)
            {
                return maxId.Value;
            } else
            {
                return 0;
            }
        }

        public int GetGreatestTrackId()
        {
            int? maxId = db.Tracks.Max(u => (int?)u.Id);

            if(maxId.HasValue)
            {
                return maxId.Value;
            } else
            {
                return 0;
            }
        }
    }
}
