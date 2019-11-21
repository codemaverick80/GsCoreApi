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
            if (artist == null) { throw new  ArgumentNullException(nameof(artist));}
            _context.Add(artist);
        }

        public void AddArtistBasicInfo(ArtistBasicInfo basicInfo)
        {
            if (basicInfo == null) { throw new ArgumentNullException(nameof(basicInfo)); }
            
            _context.ArtistBasicInfo.Add(basicInfo);
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

        public async Task<IEnumerable<Album>> GetAlbumsByArtistAsync(Guid artistId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            var result = query.Where(a => a.ArtistId == artistId);

            return await result.ToListAsync();
        }

        public async Task<Artist> GetArtistsAsync(Guid artistId)
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

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public bool ArtistExists(Guid artistId)
        {
            return _context.Set<Artist>().Any(a => a.Id == artistId);
        }

        public bool ArtistBasicInfoExists(Guid artistId)
        {
            return _context.Set<ArtistBasicInfo>().Any(a => a.ArtistId == artistId);
        }
    }
}
