using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*
* api/v1/albums										    -> Get All albums	
* api/v1/albums/{albumId}								-> Get a single album
* api/v1/albums/{albumId}/tracks						-> Get all the tracks associated with a album
*/
namespace GsCore.Api.V1.Controllers
{
    
   // [Route("api/v{version:apiVersion}/albums")]
    [Route(ApiRoutes.AlbumsRoute.BaseUrl)]
    [ApiController]
    [ApiVersion("1.0")]
    public class AlbumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAlbumRepository _albumRepository;
       
        public AlbumController(IMapper mapper, IAlbumRepository albumRepository)
        {
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        }

        #region "Get Resources"

        [HttpGet]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbums()
        {
            var albumsEntity =await _albumRepository.GetAlbumsAsync();
            ////if (!albumsEntity.Any())
            ////{
            ////    return NotFound();
            ////}
            return Ok(_mapper.Map<AlbumGetResponse[]>(albumsEntity));
        }
        
       // [HttpGet("{albumId}",Name = "GetAlbum")]
        [HttpGet(ApiRoutes.AlbumsRoute.Get,Name= "GetAlbum")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbums(Guid albumId)
        {
            var albumsEntity =await _albumRepository.GetAlbumByIdAsync(albumId);
            if (albumsEntity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AlbumGetResponse>(albumsEntity));
        }
        
        //[HttpGet("{albumId}/tracks",Name = "GetTracksByAlbum")]
        [HttpGet(ApiRoutes.AlbumsRoute.GetTrackByAlbum, Name = "GetTracksByAlbum")]
        public async Task<ActionResult<TrackGetResponse[]>> GetTracksByAlbum(Guid albumId)
        {
            if (!_albumRepository.AlbumExists(albumId))
            {
                return NotFound();
            }

            var trackEntity = await _albumRepository.GetTracksByAlbumAsync(albumId);

            if (!trackEntity.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TrackGetResponse[]>(trackEntity));

        }

        #endregion


        #region "Create Resourse"

        [HttpPost]
        public async Task<ActionResult<AlbumGetResponse>> CreateAlbum([FromBody]AlbumCreateRequest album)
        {
            if (album.ArtistId == Guid.Empty || album.GenreId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(album));
            }

            var albumEntity = _mapper.Map<Album>(album);
            albumEntity.Id=Guid.NewGuid();

            _albumRepository.AddAlbum(albumEntity);
           await _albumRepository.SaveAsync();

            if (album.Tracks.Any())
            {
                foreach (var track in album.Tracks)
                {
                    var trackEntity = _mapper.Map<Track>(track);
                    trackEntity.Id = Guid.NewGuid();
                    trackEntity.AlbumId = albumEntity.Id;
                    _albumRepository.AddTrackToAlbum(trackEntity);
                }
                await _albumRepository.SaveAsync();
            }
            var albumResponse = _mapper.Map<AlbumGetResponse>(albumEntity);
            
           return CreatedAtRoute(
               "GetAlbum",
               new
               {
                   version = HttpContext.GetRequestedApiVersion().ToString(),
                   albumId = albumEntity.Id
               },
               albumResponse);
        }
        
       // [HttpPost("{albumId}/tracks")]
        [HttpPost(ApiRoutes.AlbumsRoute.CreateTrack)]
        public async Task<ActionResult<AlbumGetResponse>> CreateTrack(Guid albumId, [FromBody]ICollection<TrackCreateRequest> tracks)
        {
            if (!tracks.Any())
            {
                throw new ArgumentNullException(nameof(tracks));
            }

            if (!_albumRepository.AlbumExists(albumId))
            {
                return NotFound();
            }
            if (tracks.Any())
            {
                foreach (var track in tracks)
                {
                    var trackEntity = _mapper.Map<Track>(track);
                    trackEntity.AlbumId = albumId;
                    trackEntity.Id=Guid.NewGuid();
                    _albumRepository.AddTrackToAlbum(trackEntity);
                }
                await _albumRepository.SaveAsync();
            }

            return CreatedAtRoute(
                "GetTracksByAlbum",
                new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    albumId = albumId
                },null);
        }

        #endregion


        #region "Update Resourse"
        //Update Track :api/v1/albums/2/tracks/3

        //Update Album: api/v1/albums/2



        #endregion


        #region "Delete Resourse"


        //Delete Track :api/v1/albums/2/tracks/3

        //Delete Album: api/v1/albums/2


        #endregion

    }
}