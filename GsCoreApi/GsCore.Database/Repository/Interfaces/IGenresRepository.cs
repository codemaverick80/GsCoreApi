using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Database.Repository.Interfaces
{
    public interface IGenresRepository : IRepository<Genre>, IDisposable
    {
        /// <summary>
        /// Returns a single matching genre Asynchronously.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeArtists">default false</param>
        /// <returns></returns>
        Task<Genre> GetGenreAsync(int id, bool includeArtists = false);
        /// <summary>
        ///  Returns list of genres Asynchronously.
        /// </summary>
        /// <param name="includeArtists">default false</param>
        /// <param name="pageIndex">default 1</param>
        /// <param name="pageSize">default 5</param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetGenresAsync(bool includeArtists = false, int pageIndex = 1, int pageSize = 20);

        
        /// <summary>
        /// Returns a single matching genre Synchronously.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeArtists">default false</param>
        /// <returns></returns>
        Genre GetGenre(int id, bool includeArtists = false);
        /// <summary>
        ///  Returns list of genre Synchronously.
        /// </summary>
        /// <param name="includeArtists">default false</param>
        /// <param name="pageIndex">default 1</param>
        /// <param name="pageSize">default 5</param>
        /// <returns></returns>
        IEnumerable<Genre> GetGenres(bool includeArtists = false, int pageIndex = 1, int pageSize = 20);



        Task<bool> SaveChangesAsync();


    }
}
