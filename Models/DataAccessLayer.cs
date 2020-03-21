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

        /// <summary>
        /// Converts a TimeSpan object into our final string representation
        /// This string should have append a 0 to the seconds if it is less than 10,
        /// or append a 0 to the minutes if there are 1 or more hours, and min < 10
        /// </summary>
        /// <param name="time"></param>
        /// <returns>The string representation</returns>
        public string ConvertTime(TimeSpan time)
        {
            string result;
            string sec = time.Seconds <= 9 ? "0" + time.Seconds : time.Seconds.ToString();

            if(time.Hours == 0)
            {
                result = time.Minutes + ":" + sec;
            } else
            {
                string min = time.Minutes <= 9 ? "0" + time.Minutes : time.Minutes.ToString();
                result = time.Hours + ":" + min + ":" + sec;
            }

            return result;
        }

        /// <summary>
        /// This method searchs the folder passed in for any files ending in
        /// .jpg, .jpeg, .png, .gif, .tiff, .gif, .bmp with the file name cover
        /// if multiple files are found then the largest file is selected
        /// </summary>
        /// <param name="Folder">The folder to search</param>
        /// <returns>The path the album art file</returns>
        public string GetAlbumArt(string Folder)
        {
            try
            {
                Folder = Path.GetDirectoryName(Folder);
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Folder);

                if (dir.Exists)
                {
                    string[] extensions = new string[] { ".jpg", ".jpeg", ".png", ".tiff", ".gif", ".bmp" };
                    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    var queryFile = from file in fileList select file;
                    var queryExt = from file in queryFile
                                   where extensions.Contains(file.Extension)
                                   && Path.GetFileNameWithoutExtension(file.FullName).ToLower() == "cover"
                                   orderby file.Length descending
                                   select file;

                    if (queryExt.Any())
                    {
                        return queryExt.First().FullName;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        public string GetEmbedAlbumArt(TagLib.File file, string Folder)
        {
            Console.Write("Searching for embedded artwork... ");
            string path = new FileInfo(Folder).DirectoryName;
            string prePend = @"data:image/png;base64, ";
            TagLib.IPicture[] pictures = file.Tag.Pictures;
            if (pictures.Any())
            {
                Console.WriteLine("Found!");

                TagLib.IPicture picture = pictures.First();
                Image image = Image.Load(picture.Data.Data);
                using (var outputStream = new MemoryStream())
                {
                    image.SaveAsPng(outputStream);
                    return prePend + Convert.ToBase64String(outputStream.ToArray());
                }
            }

            return null;
        }
        public void AddFolder(string Folder)
        {
            string[] extensions = new string[] { ".mp3", ".flac", ".m4a", ".ape", ".wav", ".ogg", ".alac", ".aiff", ".aac" };

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
                        
                        AlbumArtPath = GetAlbumArt(fi.FullName) ?? GetEmbedAlbumArt(tFile, fi.FullName) ?? "N/A"
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
