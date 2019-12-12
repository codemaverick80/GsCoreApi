using GsCore.Api.V1.Helpers;
using GsCore.Api.V1.ResourceParameters;
using GsCore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GsCore.Api.Services.Repository.Interfaces
{
    public interface IArtistRepository: IDisposable
    {
        Task<Artist> GetArtistsAsync(Guid artistId);
       
        Task<PagedList<Artist>> GetArtistsAsync(ArtistResourceParameters artistResourceParameters);

        Task<IEnumerable<Album>> GetAlbumsByArtistAsync(Guid artistId);
        void AddArtist(Artist artist);
        void AddArtistBasicInfo(ArtistBasicInfo basicInfo);
        Task<bool> SaveAsync();
        bool ArtistExists(Guid artistId);
        bool ArtistBasicInfoExists(Guid artistId);
        void UpdateArtist(Artist artist);
        void UpdateArtistBasicInfo(ArtistBasicInfo artistBasicInfo);
        Task<ArtistBasicInfo> GetArtistBasicInfo(Guid artistId);
        void Delete(Artist artist);
    }
}
