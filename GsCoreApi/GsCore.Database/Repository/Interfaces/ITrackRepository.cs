//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using GsCore.Database.Entities;

//namespace GsCore.Database.Repository.Interfaces
//{
//    public interface ITrackRepository:IRepository<Track>,IDisposable
//    {
//        /// <summary>
//        /// Returns a single matching track Asynchronously.
//        /// </summary>
//        /// <param name="id">id</param>
//        /// <returns></returns>
//        Task<Track> GetTrackAsync(int id);
      
//        /// <summary>
//        ///  Returns list of albums Asynchronously.
//        /// </summary>
//        /// <returns></returns>
//        Task<IEnumerable<Track>> GetTracksAsync(int albumId);
//    }
//}