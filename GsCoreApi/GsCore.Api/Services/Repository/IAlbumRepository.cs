using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Api.Services.Repository
{
    public interface IAlbumRepository:IDisposable
    {

      Task<IEnumerable<Album>> GetAlbumsByArtistAsync(int artistId);

      Task<Album> GetAlbumByArtistAndAlbumAsync(int artistId,int albumId);

      Task<IEnumerable<Album>> GetAlbumsByGenreAsync(int genreId);

     //Task<Album> GetAlbumAsync(int albumId);

      void AddAlbum(int artistId, int genreId, Album album);




      Task<bool> SaveAsync();

      bool ArtistExists(int artistId);
    }
}
