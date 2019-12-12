using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GsCore.Database.Entities;
namespace GsCore.Api.Services.Repository.Interfaces
{
    public interface IAlbumRepository:IDisposable
    {
      Task<Album> GetAlbumAsync(Guid albumId);
      Task<Track> GetTrack(Guid albumId, Guid trackId);
      Task<IEnumerable<Album>> GetAlbumsAsync(int pageIndex = 1, int pageSize = 10);
      Task<IEnumerable<Track>> GetTracksByAlbumAsync(Guid albumId);
      Task<Track> GetTrack(Guid trackId);
      void AddAlbum(Album album);
      void AddTrackToAlbum(Track track);
      Task<bool> SaveAsync();
      bool AlbumExists(Guid albumId);
      bool TrackExists(Guid trackId, Guid albumId);
      void UpdateAlbum(Album album);
      void UpdateTrack(Track track);
      void Delete(Album album);
      void DeleteTrack(Track track);
     
    }
}
