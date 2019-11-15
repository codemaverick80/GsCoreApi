using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
namespace GsCore.Api.Services.Repository.Interfaces
{
   public interface IArtistRepository: IDisposable
    {
        Task<Artist> GetArtistsAsync(int artistId);
        Task<IEnumerable<Artist>> GetArtistsAsync(int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<Album>> GetAlbumsByArtistAsync(int artistId);
        void AddArtist(Artist artist);
        void AddArtistBasicInfo(ArtistBasicInfo basicInfo);
        Task<bool> SaveAsync();
        bool ArtistExists(int artistId);
        bool ArtistBasicInfoExists(int artistId);
    }
}
