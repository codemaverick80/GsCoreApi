using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.V1.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GsCore.Api.V1.Controllers
{
    ////NOTE: Albums can only be retrieved thru artist, since artist and album has parent/child relation.
    ////Api Endpoint: api/v1/artists/106/albums
    [Route("api/v{api-version:apiVersion}/artists/{artistId}/albums")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AlbumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GsCore.Api.Services.Repository.IAlbumRepository _albumRepository;
       


        public AlbumController(IMapper mapper, GsCore.Api.Services.Repository.IAlbumRepository albumRepository)
        {
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));

        }

        ////Api Endpoint: api/v1/artists/106/albums
        [HttpGet]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbumsForArtist(int artistId)
        {
            if (!_albumRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var albumsForArtistFromRepo =await _albumRepository.GetAlbumsByArtistAsync(artistId);

            return Ok(_mapper.Map<AlbumGetResponse[]>(albumsForArtistFromRepo));

        }

        ////Api Endpoint: api/v1/artists/106/albums/454
        [HttpGet("{albumId}")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbumForArtist(int artistId, int albumId)
        {
            if (!_albumRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var albumForArtistFromRepo = await _albumRepository.GetAlbumByArtistAndAlbumAsync(artistId, albumId);

            if (albumForArtistFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AlbumGetResponse>(albumForArtistFromRepo));

        }

    }
}