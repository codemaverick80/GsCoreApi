using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Api.Services.Repository
{
    public interface IAlbumRepository:IDisposable
    {

      Task<IEnumerable<Album>> GetAlbumsAsync(int artistId);

      Task<Album> GetAlbumsAsync(int artistId,int albumId);

     Task<Album> GetAlbumAsync(int albumId);

      void AddAlbum(int artistId, int genreId, Album album);

      Task<bool> SaveAsync();

      bool ArtistExists(int artistId);
    }
}
