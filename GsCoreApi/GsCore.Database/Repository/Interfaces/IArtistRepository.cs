using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Database.Repository.Interfaces
{
    public interface IArtistRepository:IRepository<Artist>,IDisposable
    {
      /// <summary>
      ///  Returns list of artist Asynchronously.
      /// </summary>
      /// <param name="includeAlbums">default false</param>
      /// <param name="pageIndex">default 1</param>
      /// <param name="pageSize">default 5</param>
      /// <returns></returns>
        Task<IEnumerable<Artist>> GetArtistsAsync(bool includeAlbums = false, int pageIndex = 1, int pageSize = 5);
        
        /// <summary>
        /// Returns a single matching artist Asynchronously.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeAlbums">default false</param>
        /// <returns></returns>
        Task<Artist> GetArtistAsync(int id, bool includeAlbums=false);

     
        /// <summary>
        /// Returns a single matching artist Synchronously.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeAlbums">default false</param>
        /// <returns></returns>
        Artist GetArtist(int id, bool includeAlbums = false);
        /// <summary>
        ///  Returns list of artist Synchronously.
        /// </summary>
        /// <param name="includeAlbums">default false</param>
        /// <param name="pageIndex">default 1</param>
        /// <param name="pageSize">default 5</param>
        /// <returns></returns>
        IEnumerable<Artist> GetArtists(bool includeAlbums = false, int pageIndex = 1, int pageSize = 5);


    }
}
