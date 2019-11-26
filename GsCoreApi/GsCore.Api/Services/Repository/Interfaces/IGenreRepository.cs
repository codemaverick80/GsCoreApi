using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;

namespace GsCore.Api.Services.Repository.Interfaces
{
   public interface IGenreRepository : IDisposable
   {

     Task<IEnumerable<Genre>>  GetGenres();

     Task<Genre> GetGenre(Guid genreId);

     Task<IEnumerable<Album>> GetAlbumsByGenre(Guid genreId);

     void AddGenre(Genre genre);

     Task<bool> SaveAsync();


     void UpdateGenre(Genre genre);
   }
}
