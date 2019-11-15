//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using GsCore.Database.Entities;

//namespace GsCore.Database.Repository.Interfaces
//{
//    public interface IAlbumRepository:IRepository<Album>,IDisposable
//    {
//        /// <summary>
//        /// Returns a single matching album Asynchronously.
//        /// </summary>
//        /// <param name="id">id</param>
//        /// <param name="includeTracks">default false</param>
//        /// <returns></returns>
//        Task<Album> GetAlbumAsync(int id, bool includeTracks = false);
      
//       /// <summary>
//       ///  Returns list of albums Asynchronously.
//       /// </summary>
//       /// <param name="includeTracks">default false</param>
//       /// <param name="pageIndex">default 1</param>
//       /// <param name="pageSize">default 5</param>
//       /// <returns></returns>
//       Task<IEnumerable<Album>> GetAlbumsAsync(bool includeTracks = false, int pageIndex = 1, int pageSize = 10);
        
//        /// <summary>
//        /// Returns a single matching albums Synchronously.
//        /// </summary>
//        /// <param name="id">id</param>
//        /// <param name="includeAlbums">default false</param>
//        /// <returns></returns>
//        Album GetAlbum(int id, bool includeAlbums = false);
//        /// <summary>
//        ///  Returns list of albums Synchronously.
//        /// </summary>
//        /// <param name="includeTracks">default false</param>
//        /// <param name="pageIndex">default 1</param>
//        /// <param name="pageSize">default 5</param>
//        /// <returns></returns>
//        IEnumerable<Album> GetAlbums(bool includeTracks = false, int pageIndex = 1, int pageSize = 5);


//        Task<IEnumerable<Album>> GetAlbumAsync(int artistId);

//        Task<Album> GetAlbumAsync(int artistId, int albumId);

//    }
//}
