using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test02.Models
{
    public class DataAccessLayer
    {
        ThingContext db = new ThingContext();

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
    }
}
