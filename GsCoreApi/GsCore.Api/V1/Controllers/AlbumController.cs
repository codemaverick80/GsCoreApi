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
    //[Route("api/v{api-version:apiVersion}/artists/{artistId}/albums")]
    [Route("api/v{version:apiVersion}/albums")]
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
        
        [HttpGet("{albumId}",Name = "GetAlbum")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbums(int albumId)
        {
            var albumsEntity =await _albumRepository.GetAlbumByIdAsync(albumId);
            if (albumsEntity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AlbumGetResponse>(albumsEntity));
        }
        
        [HttpGet("{albumId}/tracks",Name = "GetTracksByAlbum")]
        public async Task<ActionResult<TrackGetResponse[]>> GetTracksByAlbum(int albumId)
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
            if (album.ArtistId == 0 || album.GenreId == 0)
            {
                throw new ArgumentNullException(nameof(album));
            }

            var albumEntity = _mapper.Map<Album>(album);

            _albumRepository.AddAlbum(albumEntity);
           await _albumRepository.SaveAsync();

           if (album.Tracks.Any())
           {
               foreach (var track in album.Tracks)
               { 
                   var trackEntity = _mapper.Map<Track>(track);
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
                   albumId = albumResponse.Id
               },
               albumResponse);
        }
        
        [HttpPost("{albumId}/tracks")]
        public async Task<ActionResult<AlbumGetResponse>> CreateTrack(int albumId, [FromBody]ICollection<TrackCreateRequest> tracks)
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

        #endregion


        #region "Delete Resourse"

        #endregion

    }
}