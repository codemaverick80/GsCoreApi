using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GsCore.Api.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/genres")]
    [ApiVersion("1.0")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _mapper = mapper;
            _genreRepository = genreRepository;
        }
        [HttpGet()]
        public async Task<ActionResult<GenreGetResponse[]>> Get()
        {
            var result = await _genreRepository.GetGenres();
            //return _mapper.Map<GenreGetResponse[]>(result);
            return Ok(_mapper.Map<GenreGetResponse[]>(result));

        }

        [HttpGet("{genreId}",Name = "GetGenre")]
        public async Task<ActionResult<GenreGetResponse>> Get(int genreId)
        {
            var result = await _genreRepository.GetGenre(genreId);
            if (result == null) return NotFound();
            return Ok(_mapper.Map<GenreGetResponse>(result));

        }
        //api/v1/genres/{genreId}/albums
        [HttpGet("{genreId}/albums")]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbumsByGenre(int genreId)
        {

            var result = await _genreRepository.GetAlbumsByGenre(genreId);
           
            if (!result.Any()) return NotFound();

            return Ok(_mapper.Map<AlbumGetResponse[]>(result));

        }

        [HttpPost]
        public async Task<ActionResult<GenreGetResponse>> CreateGenre(GenreCreateRequest genre)
        {
             var genreEntity = _mapper.Map<Genre>(genre);
             _genreRepository.AddGenre(genreEntity);
             await  _genreRepository.SaveAsync();

             var genreGetResponse = _mapper.Map<GenreGetResponse>(genreEntity);

             //// return Created(GetResourceUrl(genreGetResponse.Id.ToString()), genreGetResponse);

             //// OR
            
             return CreatedAtRoute(
                 "GetGenre",
                 new
                 {
                     version = HttpContext.GetRequestedApiVersion().ToString(), 
                     genreId = genreGetResponse.Id
                 }, 
                 genreGetResponse);

        }



        /// <summary>
        /// Indicate the URL to redirect a page to
        /// </summary>
        /// <param name="resourceId">currently created resource id returned from data source.</param>
        /// <returns></returns>
        private string GetResourceUrl(string resourceId)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"{HttpContext.Request.Path}/" + resourceId;
            return locationUrl;
        }



    }
}