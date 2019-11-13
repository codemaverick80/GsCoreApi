using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Api.Services.Repository
{
    public class AlbumRepository:IAlbumRepository
    {
        private readonly GsDbContext _context;

        public AlbumRepository(GsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<Album> GetAlbumAsync(int albumId)
        //{
        //    IQueryable<Album> query = _context.Set<Album>();
            
        //    //// Build Query
        //    var result = query.Where(a => a.Id == albumId);


        //    //// execute query and return the result
        //    return await result.FirstOrDefaultAsync();

        //}



        public async Task<IEnumerable<Album>> GetAlbumsByArtistAsync(int artistId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            //// Build Query
            var result = query.Where(a => a.ArtistId == artistId);

            //// execute query and return the result
            return await result.ToListAsync();
        }

        public async Task<Album> GetAlbumByArtistAndAlbumAsync(int artistId, int albumId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            //// Build Query
            var result = query.Where(a => a.ArtistId == artistId && a.Id == albumId);

            //// execute query and return the result
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByGenreAsync(int genreId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            var result = query.Where(a => a.GenreId == genreId);

            return await query.ToListAsync();
        }

       

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }


        public void AddAlbum(int artistId,int genreId, Album album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }

            album.ArtistId = artistId;
            album.GenreId = genreId;
            _context.Add(album);
        }


        public bool ArtistExists(int artistId)
        {
           return _context.Set<Artist>().Any(a => a.Id == artistId);
            ////return _context.Artist.Any(a => a.Id == artistId);
        }



        #region "Disposing"

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        #endregion

    }
}
