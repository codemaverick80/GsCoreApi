using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Api.V1.Helpers;
using GsCore.Api.V1.ResourceParameters;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Api.Services.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly GsDbContext _context;
        private IPropertyMappingService _propertyMappingService;

        public ArtistRepository(GsDbContext context, IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService??throw new ArgumentNullException(nameof(propertyMappingService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void AddArtist(Artist artist)
        {
            if (artist == null) { throw new ArgumentNullException(nameof(artist)); }
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
                .Include(a => a.ArtistBasicInfo)
                .Where(a => a.Id == artistId);

            return await result.FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<Artist>> GetArtistsAsync(int pageIndex = 1, int pageSize = 10)
        //{
        //    IQueryable<Artist> query = _context.Set<Artist>();

        //    var result=query
        //        .Include(a => a.ArtistBasicInfo)
        //        .Skip((pageIndex - 1) * pageSize)
        //        .Take(pageSize);

        //    return await result.ToListAsync();
        //}

        public async Task<PagedList<Artist>> GetArtistsAsync(ArtistResourceParameters artistResourceParameters)
        {
            //if (artistResourceParameters == null)
            //{
            //    throw new ArgumentNullException(nameof(artistResourceParameters));
            //}

            IQueryable<Artist> query = _context.Set<Artist>();

            if (!string.IsNullOrWhiteSpace(artistResourceParameters.SearchQuery))
            {
                artistResourceParameters.SearchQuery = artistResourceParameters.SearchQuery.Trim();
                query = query
                    .Include(a => a.ArtistBasicInfo)
                    .Where(a => a.FirstName.Contains(artistResourceParameters.SearchQuery) 
                                || a.LastName.Contains(artistResourceParameters.SearchQuery));
            }

            #region "Sorting Artist"


            #region "Not very useful and reusable"

            ////if (!string.IsNullOrWhiteSpace(artistResourceParameters.OrderBy))
            ////{
            ////    if (artistResourceParameters.OrderBy.ToLowerInvariant() == "name")
            ////    {
            ////        query = query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
            ////    }
            ////}
            
            #endregion

            //// Install:  System.Linq.Dynamic.Core
            //// PropertyMappingValue.cs
            //// PropertyMappingService.cs (IPropertyMappingService.cs)
            //// PropertyMapping.cs (IPropertyMapping.cs)

            if (!string.IsNullOrWhiteSpace(artistResourceParameters.OrderBy)) 
            {
               // get property mapping dictionary
                var artistPropertyMappingDictionary =_propertyMappingService.GetPropertyMapping<ArtistGetResponse, Artist>();

                query=query.ApplySort(artistResourceParameters.OrderBy, artistPropertyMappingDictionary);
            }

            #endregion


            #region "Pagination"
            /* ========== Pagination should be added at the end ========== */

            #region "Basic Pagination"
            ////query = query
            ////     .Skip(artistResourceParameters.PageSize * (artistResourceParameters.PageNumber - 1))
            ////     .Take(artistResourceParameters.PageSize);
            ////return await query.ToListAsync();
            #endregion

            return PagedList<Artist>.Create(query, artistResourceParameters.PageNumber, artistResourceParameters.PageSize);

            #endregion

           
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

        public void UpdateArtist(Artist artist)
        {
            //no code implemented
        }

        public async Task<ArtistBasicInfo> GetArtistBasicInfo(Guid artistId)
        {
            IQueryable<ArtistBasicInfo> query = _context.Set<ArtistBasicInfo>();

            var result = query.Where(a => a.ArtistId == artistId);

            return await result.FirstOrDefaultAsync();
        }

        public void UpdateArtistBasicInfo(ArtistBasicInfo artistBasicInfo)
        {
            //no code implemented
        }

        public void Delete(Artist artist)
        {
            _context.Artist.Remove(artist);
            if (artist.ArtistBasicInfo != null)
            {
                _context.ArtistBasicInfo.Remove(artist.ArtistBasicInfo);
            }
        }
    }
}
