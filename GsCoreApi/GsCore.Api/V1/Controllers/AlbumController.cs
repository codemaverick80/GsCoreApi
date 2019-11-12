using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GsCore.Api.V1.Controllers
{
    ////Api Endpoint: api/v1/artists/106/albums
    [Route("api/v{api-version:apiVersion}/artists/{artistId}/albums")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AlbumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;


        public AlbumController(IMapper mapper,IAlbumRepository albumRepository, IArtistRepository artistRepository)
        {
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _albumRepository = albumRepository?? throw new ArgumentNullException(nameof(albumRepository));
            _artistRepository = artistRepository?? throw new ArgumentNullException(nameof(artistRepository));
        }

        ////Api Endpoint: api/v1/artists/106/albums
        [HttpGet]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbumsForArtist(int artistId)
        {
            if (!_artistRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var albumsForArtistFromRepo =await _albumRepository.GetAlbumAsync(artistId);

            return Ok(_mapper.Map<AlbumGetResponse[]>(albumsForArtistFromRepo));

        }

        ////Api Endpoint: api/v1/artists/106/albums/454
        [HttpGet("{albumId}")]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbumForArtist(int artistId, int albumId)
        {
            if (!_artistRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var albumForArtistFromRepo = await _albumRepository.GetAlbumAsync(artistId, albumId);

            if (albumForArtistFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AlbumGetResponse>(albumForArtistFromRepo));

        }

    }
}