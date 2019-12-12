using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GsCore.Api.Services.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly GsDbContext _context;

        public GenreRepository(GsDbContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Album>> GetAlbumsByGenre(Guid genreId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            var result = query.Where(album => album.GenreId == genreId);

            return await result.ToListAsync();
        }

        public async Task<Genre> GetGenre(Guid genreId)
        {
            IQueryable<Genre> query = _context.Set<Genre>();

            var result = query.Where(g => g.Id == genreId);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            IQueryable<Genre> query = _context.Set<Genre>();

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
        
        public void AddGenre(Genre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }
            _context.Add(genre);
        }


        public void UpdateGenre(Genre genre)
        {
            //no code implemented
        }

        public bool GenreExists(Guid genreId)
        {
            return _context.Set<Genre>().Any(a => a.Id == genreId);
        }


        public void Delete(Genre genre)
        {
            _context.Genre.Remove(genre);
        }

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
    }
}
