using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
using GsCore.Database.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Database.Repository.Implementation
{
    public class AlbumRepository:Repository<Album>,IAlbumRepository
    {
        public AlbumRepository(GsDbContext context):base(context)
        {
        }

        public async Task<Album> GetAlbumAsync(int id,bool includeTracks = false)
        {
            IQueryable<Album> query = FindAll();

            if (includeTracks)
            {
                query = query.Include(a => a.Track);
            }

            //Query it
            var result = query.Where(a => a.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync(bool includeTracks = false, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Album> query = FindAll();
            if (includeTracks)
            {
                query = query.Include(a => a.Track);
            }
            //Query it
            var result = query.OrderBy(a => a.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return await result.ToListAsync();
        }

        public Album GetAlbum(int id, bool includeTracks = false)
        {
            //_context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            IQueryable<Album> query = FindAll();
            if (includeTracks)
            {
                query = query.Include(genre => genre.Track);
            }
          var result = query.Where(g => g.Id == id);
           return result.SingleOrDefault();
        }

        public IEnumerable<Album> GetAlbums(bool includeTracks = false, int pageIndex = 1, int pageSize = 5)
        {
            IQueryable<Album> query = FindAll();
            if (includeTracks)
            {
                query = query.Include(artist => artist.Track);
            }
           var result= query.OrderBy(a => a.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return result.ToList();
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
                //_context = null;
            }
        }


        #endregion
    }
}
