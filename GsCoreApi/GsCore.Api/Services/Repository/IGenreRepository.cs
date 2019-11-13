using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1;
using GsCore.Database.Entities;

namespace GsCore.Api.Services.Repository
{
   public interface IGenreRepository : IDisposable
   {

     Task<IEnumerable<Genre>>  GetGenres();

     Task<Genre> GetGenre(int genreId);

     Task<IEnumerable<Album>> GetAlbumsByGenre(int genreId);

     void AddGenre(Genre genre);

     Task<bool> SaveAsync();
    }
}
