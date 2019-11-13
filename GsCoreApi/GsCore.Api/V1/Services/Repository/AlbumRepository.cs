using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Api.V1.Services.Repository
{
    public class AlbumRepository:IAlbumRepository
    {
        private readonly GsDbContext _context;

        public AlbumRepository(GsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Album> GetAlbum(int albumId)
        {
            IQueryable<Album> query = _context.Set<Album>();
            
            //// Build Query
            var result = query.Where(a => a.Id == albumId);


            //// execute query and return the result
            return await result.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Album>> GetAlbums(int artistId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            //// Build Query
            var result = query.Where(a => a.ArtistId == artistId);

            //// execute query and return the result
            return await result.ToListAsync();
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


        public async Task<bool> Save()
        {
            return ( await _context.SaveChangesAsync() > 0);
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
