using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GsCore.Database.Entities;
using GsCore.Database.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Database.Repository.Implementation
{
    public class GenresRepository: Repository<Genre>, IGenresRepository
    {

        #region "Constructor"

        public GenresRepository(GsDbContext context):base(context)
        {

        }   

        #endregion


        #region "Asynchronous Methods"
        
        public async Task<IEnumerable<Genre>> GetGenresAsync(bool includeAlbum = false, int pageIndex = 1, int pageSize = 20)
        {
            IQueryable<Genre> query = FindAll();

            if (includeAlbum)
            {
                query = query.Include(genres => genres.Album);
            }

            //Query It
          var result=  query.OrderBy(g=>g.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return await result.ToListAsync();
        }


        public async Task<Genre> GetGenreAsync(int id, bool includeAlbum = false)
        {
            //_context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            IQueryable<Genre> query = FindAll();
            if (includeAlbum)
            {
                query = query.Include(genre => genre.Album);
            }

            // Query It
            var result = query.Where(g => g.Id == id);

            return await result.SingleOrDefaultAsync();
        }

        #endregion

        #region "Synchronous Methods"
        public Genre GetGenre(int id, bool includeAlbum = false)
        {
            //_context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            IQueryable<Genre> query = FindAll();
            if (includeAlbum)
            {
                query = query.Include(genre => genre.Album);
            }
            var result = query.Where(g => g.Id == id);
            return result.SingleOrDefault();
        }

        public IEnumerable<Genre> GetGenres(bool includeAlbum = false, int pageIndex = 1, int pageSize = 20)
        {
            IQueryable<Genre> query = FindAll();
            if (includeAlbum)
            {
                query = query.Include(genres => genres.Album);
            }
            //Query It
          var result=  query.OrderBy(g=>g.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return result.ToList();
        }


        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }

        #endregion

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
                if (_context != null)
                {
                    _context.Dispose();
                    //_context = null;
                }
            }
        }


        #endregion

    }
}


//public async Task<IEnumerable<Genres>> GetAllGenresAsync(bool includeArtists = false, bool includeSubGenres = false)
//{
//    IQueryable<Genres> query = FindAll();
//    if (includeArtists)
//    {
//        query = query.Include(g => g.Artists);
//    }
//    if (includeSubGenres)
//    {
//        query = query.Include(g => g.SubGenres);
//    }

//    return await query.OrderBy(g => g.Id).ToListAsync();
//}


//public IEnumerable<Genres> FindWithSubGenres(Func<Genres, bool> predicate)
//{
//    return _context.Genres
//        .Include(a => a.SubGenres)
//        .Where(predicate).ToList();
//}

//public IEnumerable<Genres> GetAllWithSubGenres()
//{
//    return _context.Genres.Include(a => a.SubGenres);
//}
