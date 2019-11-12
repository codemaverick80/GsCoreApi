using AutoMapper;
using GsCore.Api.Contracts.v2;
using GsCore.Api.Contracts.v2.Responses;
using GsCore.Database.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GsCore.Api.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/genres")]
    [ApiVersion("2.0")]
    public class GenreController : ControllerBase
    {
        private readonly IGenresRepository _genresRepository;
        private IMapper _mapper;

        public GenreController(IGenresRepository genresRepository, IMapper mapper)
        {
            _mapper = mapper;
            _genresRepository = genresRepository;
        }

       
        [HttpGet()]
        public async Task<ActionResult<GenreGetResponse[]>> Get()
        {
            var result = await _genresRepository.GetGenresAsync(false, 1, 5);
            //return _mapper.Map<GenreGetResponse[]>(result);
            return Ok(_mapper.Map<GenreGetResponse[]>(result));

        }

        
        [HttpGet("{genreId}")]
        public async Task<ActionResult<GenreGetResponse[]>> Get(int genreId)
        {
            var result = await _genresRepository.GetGenreAsync(genreId, false);
            if (result == null) return NotFound();
            return Ok(_mapper.Map<GenreGetResponse>(result));

        }
    }
}