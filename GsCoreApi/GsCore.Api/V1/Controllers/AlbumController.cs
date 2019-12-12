using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/*
* api/v1/albums										    -> Get All albums	
* api/v1/albums/{albumId}								-> Get a single albumPostRequest
* api/v1/albums/{albumId}/tracks						-> Get all the tracks associated with a albumPostRequest
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
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        }

        #region "GET Request"

        [HttpGet]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbums()
        {
            var albumsEntity = await _albumRepository.GetAlbumsAsync();
            ////if (!albumsEntity.Any())
            ////{
            ////    return NotFound();
            ////}
            return Ok(_mapper.Map<AlbumGetResponse[]>(albumsEntity));
        }

        // [HttpGet("{albumId}",Name = "GetAlbum")]
        [HttpGet(ApiRoutes.AlbumsRoute.Get, Name = "GetAlbum")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbums(Guid albumId)
        {
            var albumsEntity = await _albumRepository.GetAlbumAsync(albumId);
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

        #region "POST Request"

        [HttpPost]
        public async Task<ActionResult<AlbumGetResponse>> CreateAlbum([FromBody]AlbumPostRequest albumPostRequest)
        {
            if (albumPostRequest.ArtistId == Guid.Empty || albumPostRequest.GenreId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(albumPostRequest));
            }

            var albumToAdd = _mapper.Map<Album>(albumPostRequest);
            albumToAdd.Id = Guid.NewGuid();

            _albumRepository.AddAlbum(albumToAdd);
            await _albumRepository.SaveAsync();

            if (albumPostRequest.Tracks.Any())
            {
                foreach (var track in albumPostRequest.Tracks)
                {
                    var trackToAdd = _mapper.Map<Track>(track);
                    trackToAdd.Id = Guid.NewGuid();
                    trackToAdd.AlbumId = albumToAdd.Id;
                    _albumRepository.AddTrackToAlbum(trackToAdd);
                }
                await _albumRepository.SaveAsync();
            }
            var albumResponse = _mapper.Map<AlbumGetResponse>(albumToAdd);

            return CreatedAtRoute(
                "GetAlbum",
                new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    albumId = albumToAdd.Id
                },
                albumResponse);
        }

        // [HttpPost("{albumId}/tracks")]
        [HttpPost(ApiRoutes.AlbumsRoute.PostTrack)]
        public async Task<ActionResult<AlbumGetResponse>> CreateTrack(Guid albumId, [FromBody]ICollection<TrackPostRequest> tracks)
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
                    var trackToAdd = _mapper.Map<Track>(track);
                    trackToAdd.AlbumId = albumId;
                    trackToAdd.Id = Guid.NewGuid();
                    _albumRepository.AddTrackToAlbum(trackToAdd);
                }
                await _albumRepository.SaveAsync();
            }

            return CreatedAtRoute(
                "GetTracksByAlbum",
                new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    albumId = albumId
                }, null);
        }

        #endregion

        #region "PUT Request"

        //PUT: api/v1/albums/2
        [HttpPut(ApiRoutes.AlbumsRoute.PutAlbum)]
        public async Task<ActionResult> UpdateAlbum(Guid albumId, [FromBody] AlbumPutRequest albumPutRequest)
        {

            if (!_albumRepository.AlbumExists(albumId))
            {
                //TODO: Create new album

                //map source (albumPutRequest) to destination type <album>
                var albumToAdd = _mapper.Map<Album>(albumPutRequest);
                albumToAdd.Id = albumId;

                //call add
                _albumRepository.AddAlbum(albumToAdd);
                //call save
                await _albumRepository.SaveAsync();

                //map source to  AlbumGetResponse type
                var albumGetResponse = _mapper.Map<AlbumGetResponse>(albumToAdd);

                //create response and route
                return CreatedAtRoute("GetAlbum", new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    artistId = albumGetResponse.Id
                },
                    albumGetResponse);
            }

            //Get album from repo
            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);

            //map source (albumPutRequest) to existing destination (albumFromRepo)
            _mapper.Map(albumPutRequest, albumFromRepo);

            //call update
            _albumRepository.UpdateAlbum(albumFromRepo);
            //call save
            await _albumRepository.SaveAsync();

            return NoContent();
        }

        //PUT: api/v1/albums/2/tracks/3
        [HttpPut(ApiRoutes.AlbumsRoute.PutTrack)]
        public async Task<ActionResult> UpdateTrack(Guid trackId, Guid albumId, [FromBody] TrackPutRequest trackPutRequest)
        {
            if (!_albumRepository.AlbumExists(albumId))
            {
                return NotFound();
            }

            if (!_albumRepository.TrackExists(trackId, albumId))
            {
                var trackToAdd = _mapper.Map<Track>(trackPutRequest);
                trackToAdd.Id = trackId;
                trackToAdd.AlbumId = albumId;
                _albumRepository.AddTrackToAlbum(trackToAdd);
                await _albumRepository.SaveAsync();
                return CreatedAtRoute(
                    "GetTracksByAlbum",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        albumId = albumId
                    }, null);
            }

            var trackFromRepo = await _albumRepository.GetTrack(trackId);
            trackFromRepo.AlbumId = albumId;
            _mapper.Map(trackPutRequest, trackFromRepo);
            trackFromRepo.AlbumId = albumId;
            _albumRepository.UpdateTrack(trackFromRepo);
            await _albumRepository.SaveAsync();

            return NoContent();
        }

        #endregion

        #region "PATCH Request"

        [HttpPatch(ApiRoutes.AlbumsRoute.PatchAlbum)]
        public async Task<ActionResult> PartialAlbumUpdate(Guid albumId, JsonPatchDocument<AlbumPatchRequest> patchRequest)
        {
            if (!_albumRepository.AlbumExists(albumId))
            {
                var albumPatch = new AlbumPatchRequest();
                patchRequest.ApplyTo(albumPatch, ModelState);

                //TODO: Validation 
                if (!TryValidateModel(albumPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var albumToAdd = _mapper.Map<Album>(albumPatch);
                albumToAdd.Id = albumId;

                _albumRepository.AddAlbum(albumToAdd);

                await _albumRepository.SaveAsync();

                var albumGetResponse = _mapper.Map<AlbumGetResponse>(albumToAdd);
                return CreatedAtRoute(
                    "GetAlbum",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = albumGetResponse.Id
                    },
                    albumGetResponse);
            }

            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
            var albumToPatch = _mapper.Map<AlbumPatchRequest>(albumFromRepo);

            patchRequest.ApplyTo(albumToPatch);
            //TODO: Validation
            if (!TryValidateModel(albumToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(albumToPatch, albumFromRepo);
            _albumRepository.UpdateAlbum(albumFromRepo);
            await _albumRepository.SaveAsync();
            return NoContent();


        }

        [HttpPatch(ApiRoutes.AlbumsRoute.PatchTrack)]
        public async Task<ActionResult> PartialTrackUpdate(Guid albumId, Guid trackId, JsonPatchDocument<TrackPatchRequest> patchRequest)
        {
            if (!_albumRepository.AlbumExists(albumId))
            {
                return NotFound();
            }

            if (!_albumRepository.TrackExists(trackId, albumId))
            {
                //return NotFound();
                var trackPatch = new TrackPatchRequest();
                patchRequest.ApplyTo(trackPatch, ModelState);

                //TODO: Validation 
                if (!TryValidateModel(trackPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var trackToAdd = _mapper.Map<Track>(trackPatch);
                trackToAdd.Id = trackId;
                trackToAdd.AlbumId = albumId;

                _albumRepository.AddTrackToAlbum(trackToAdd);
                await _albumRepository.SaveAsync();

                var fromRepo = await _albumRepository.GetTrack(trackId);
                var trackGetResponse = _mapper.Map<TrackGetResponse>(fromRepo);
                return CreatedAtRoute(
                    "GetTracksByAlbum",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        albumId = trackGetResponse.AlbumId
                    },
                    trackGetResponse);

            }

            var trackFromRepo = await _albumRepository.GetTrack(trackId);
            var trackToPatch = _mapper.Map<TrackPatchRequest>(trackFromRepo);

            patchRequest.ApplyTo(trackToPatch);
            //TODO: Validation
            if (!TryValidateModel(trackToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(trackToPatch, trackFromRepo);
            _albumRepository.UpdateTrack(trackFromRepo);
            await _albumRepository.SaveAsync();
            return NoContent();
        }

        #endregion

        #region "DELETE Request"

        /* DO NOT expose delete endpoint */
        //////Delete Track :api/v1/albums/2/tracks/3

        ////[HttpDelete("{albumId}/tracks/{trackId}")]
        ////public async Task<ActionResult> DeleteTrack(Guid albumId, Guid trackId)
        ////{
        ////    var trackFromRepo = await _albumRepository.GetTrack(albumId, trackId);
        ////    if (trackFromRepo == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    _albumRepository.DeleteTrack(trackFromRepo);
        ////    await _albumRepository.SaveAsync();
        ////    return NoContent();
        ////}
        //////Delete Album: api/v1/albums/2

        ////[HttpDelete("{albumId}")]
        ////public async Task<ActionResult> Delete(Guid albumId)
        ////{
        ////    var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
        ////    if (albumFromRepo == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    _albumRepository.Delete(albumFromRepo);
        ////    await _albumRepository.SaveAsync();
        ////    return NoContent();
        ////}


        #endregion


        public override ActionResult ValidationProblem(
            [ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }


    }
}