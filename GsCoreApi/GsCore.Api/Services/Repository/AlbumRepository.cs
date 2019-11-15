using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.Services.Repository.Interfaces;
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

        public async Task<Album> GetAlbumByIdAsync(int albumId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            //// Build Query
            var result = query.Where(a => a.Id == albumId);


            //// execute query and return the result
            return await result.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync(int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Album> query = _context.Set<Album>();

            //Query it
            var result = query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return await result.ToListAsync();
        }



        public async Task<IEnumerable<Track>> GetTracksByAlbumAsync(int albumId)
        {
            IQueryable<Track> query = _context.Set<Track>();

            var result = query.Where(t => t.AlbumId == albumId);

            return await result.ToListAsync();
        }


        public bool AlbumExists(int albumId)
        {
            return _context.Set<Album>().Any(a => a.Id ==albumId);
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
