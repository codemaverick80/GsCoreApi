using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.V2.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
namespace GsCore.Api.V2.Controllers
{
    //[ApiController]
    //[Route("api/v{api-version:apiVersion}/genres")]
    //[ApiVersion("2.0")]
    //public class GenreController : ControllerBase
    //{
    //    private readonly IGenresRepository _genresRepository;
    //    private IMapper _mapper;

    //    public GenreController(IGenresRepository genresRepository, IMapper mapper)
    //    {
    //        _mapper = mapper;
    //        _genresRepository = genresRepository;
    //    }

       
    //    [HttpGet()]
    //    public async Task<ActionResult<GenreGetResponse[]>> Get()
    //    {
    //        var result = await _genresRepository.GetGenresAsync(false, 1, 5);
    //        //return _mapper.Map<GenreGetResponse[]>(result);
    //        return Ok(_mapper.Map<GenreGetResponse[]>(result));

    //    }

        
    //    [HttpGet("{genreId}")]
    //    public async Task<ActionResult<GenreGetResponse[]>> Get(int genreId)
    //    {
    //        var result = await _genresRepository.GetGenreAsync(genreId, false);
    //        if (result == null) return NotFound();
    //        return Ok(_mapper.Map<GenreGetResponse>(result));

    //    }
    //}
}