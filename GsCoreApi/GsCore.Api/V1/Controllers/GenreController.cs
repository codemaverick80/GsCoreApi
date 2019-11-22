using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace GsCore.Api.V1.Controllers
{
    [ApiController]
   // [Route("api/v{version:apiVersion}/genres")]
    [Route(ApiRoutes.GenresRoute.BaseUrl)]
    [ApiVersion("1.0")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _genreRepository = genreRepository??throw new ArgumentNullException(nameof(genreRepository));
        }
       
        /// <summary>
        /// Provides genres
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<GenreGetResponse[]>> Get()
        {
            var result = await _genreRepository.GetGenres();
            if (!result.Any())
            {
                return NotFound();
            }

            //return _mapper.Map<GenreGetResponse[]>(result);
            return Ok(_mapper.Map<GenreGetResponse[]>(result));

        }

        /// <summary>
        /// Provide genre by id
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.GenresRoute.Get,Name = "GetGenre")]
        public async Task<ActionResult<GenreGetResponse>> Get(Guid genreId)
        {
            var result = await _genreRepository.GetGenre(genreId);
            if (result == null) return NotFound();
            return Ok(_mapper.Map<GenreGetResponse>(result));

        }
       
      /// <summary>
      /// Provide list of albums by genre
      /// </summary>
      /// <param name="genreId"></param>
      /// <returns></returns>
        [HttpGet(ApiRoutes.GenresRoute.GetAlbumByGenre)]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbumsByGenre(Guid genreId)
        {

            var result = await _genreRepository.GetAlbumsByGenre(genreId);
           
            if (!result.Any()) return NotFound();

            return Ok(_mapper.Map<AlbumGetResponse[]>(result));

        }

      /// <summary>
      /// Creates genre
      /// </summary>
      /// <param name="genre"></param>
      /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GenreGetResponse>> CreateGenre(GenreCreateRequest genre)
        {

            var genreEntity = _mapper.Map<Genre>(genre);
            genreEntity.Id=Guid.NewGuid();
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