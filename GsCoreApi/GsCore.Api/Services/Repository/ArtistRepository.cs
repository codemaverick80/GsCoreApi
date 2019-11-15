using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Api.Services.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly GsDbContext _context;

        public ArtistRepository(GsDbContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }


        public void AddArtist(Artist artist)
        {
            throw new NotImplementedException();
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

        public async Task<IEnumerable<Album>> GetAlbumsByArtistAsync(int artistId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            var result = query.Where(a => a.ArtistId == artistId);

            return await result.ToListAsync();
        }

        public async Task<Artist> GetArtistsAsync(int artistId)
        {
            IQueryable<Artist> query = _context.Set<Artist>();

            var result = query
                .Include(a=>a.ArtistBasicInfo)
                .Where(a => a.Id == artistId);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Artist>> GetArtistsAsync(int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Artist> query = _context.Set<Artist>();

            var result=query
                .Include(a => a.ArtistBasicInfo)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return await result.ToListAsync();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
