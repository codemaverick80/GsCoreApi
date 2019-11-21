using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V2.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
namespace GsCore.Api.V2.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/genres")]
    [ApiVersion("2.0")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genresRepository, IMapper mapper)
        {
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _genreRepository = genresRepository ?? throw new ArgumentNullException(nameof(genresRepository));
        }

        [HttpGet()]
        public async Task<ActionResult<GenreGetResponse[]>> Get()
        {
            var result = await _genreRepository.GetGenres();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GenreGetResponse[]>(result));
        }

        [HttpGet("{genreId}", Name = "GetGenre")]
        public async Task<ActionResult<GenreGetResponse>> Get(Guid genreId)
        {
            var result = await _genreRepository.GetGenre(genreId);
            if (result == null) return NotFound();
            return Ok(_mapper.Map<GenreGetResponse>(result));
        }

        //api/v1/genres/{genreId}/albums
        [HttpGet("{genreId}/albums")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbumsByGenre(Guid genreId)
        { 
            var result = await _genreRepository.GetAlbumsByGenre(genreId);
            if (!result.Any()) return NotFound();
            return Ok(_mapper.Map<AlbumGetResponse[]>(result));
        }

    }
}