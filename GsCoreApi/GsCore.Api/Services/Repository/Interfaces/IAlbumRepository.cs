using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;
namespace GsCore.Api.Services.Repository.Interfaces
{
    public interface IAlbumRepository:IDisposable
    {
      Task<Album> GetAlbumByIdAsync(int albumId);
      Task<IEnumerable<Album>> GetAlbumsAsync(int pageIndex = 1, int pageSize = 10);
      Task<IEnumerable<Track>> GetTracksByAlbumAsync(int albumId);
      void AddAlbum(int artistId, int genreId, Album album);
      Task<bool> SaveAsync();
      bool AlbumExists(int albumId);
    }
}
