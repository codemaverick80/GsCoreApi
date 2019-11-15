using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Responses;
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

        [HttpGet]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbums()
        {

            var albumsEntity =await _albumRepository.GetAlbumsAsync();

            return Ok(_mapper.Map<AlbumGetResponse[]>(albumsEntity));
        }


       
        [HttpGet("{albumId}")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbums(int albumId)
        {
           
            var albumsEntity =await _albumRepository.GetAlbumByIdAsync(albumId);

            return Ok(_mapper.Map<AlbumGetResponse>(albumsEntity));

        }

       
        [HttpGet("{albumId}/tracks")]
        public async Task<ActionResult<TrackGetResponse[]>> GetTracksByAlbum(int albumId)
        {
            if (!_albumRepository.AlbumExists(albumId))
            {
                return NotFound();
            }

            var trackEntity = await _albumRepository.GetTracksByAlbumAsync(albumId);

            if (trackEntity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TrackGetResponse[]>(trackEntity));

        }





    }
}