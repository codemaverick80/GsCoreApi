using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
namespace GsCore.Api.Services.Repository.Interfaces
{
   public interface IArtistRepository: IDisposable
    {
        Task<Artist> GetArtistsAsync(Guid artistId);
        Task<IEnumerable<Artist>> GetArtistsAsync(int pageIndex = 1, int pageSize = 10);
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
