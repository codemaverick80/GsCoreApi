using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Api.Services.Repository
{
    public interface IAlbumRepository:IDisposable
    {

      Task<IEnumerable<Album>> GetAlbums(int artistId);

      Task<Album> GetAlbum(int albumId);

      void AddAlbum(int artistId, int genreId, Album album);

      Task<bool> Save();
    }
}
