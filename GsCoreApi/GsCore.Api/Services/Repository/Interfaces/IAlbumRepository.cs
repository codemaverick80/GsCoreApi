using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;
namespace GsCore.Api.Services.Repository.Interfaces
{
    public interface IAlbumRepository:IDisposable
    {
      Task<Album> GetAlbumByIdAsync(Guid albumId);
      Task<IEnumerable<Album>> GetAlbumsAsync(int pageIndex = 1, int pageSize = 10);
      Task<IEnumerable<Track>> GetTracksByAlbumAsync(Guid albumId);
     // void AddAlbum(int artistId, int genreId, Album album);
      void AddAlbum(Album album);
      void AddTrackToAlbum(Track track);
      Task<bool> SaveAsync();
      bool AlbumExists(Guid albumId);
    }
}
