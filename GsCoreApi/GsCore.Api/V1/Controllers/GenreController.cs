using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

       
        [HttpGet(ApiRoutes.GenresRoute.Get,Name = "GetGenre")]
        public async Task<ActionResult<GenreGetResponse>> Get(Guid genreId)
        {
            var result = await _genreRepository.GetGenre(genreId);
            if (result == null) return NotFound();
            return Ok(_mapper.Map<GenreGetResponse>(result));

        }
       
     
        [HttpGet(ApiRoutes.GenresRoute.GetAlbumByGenre)]
        public async Task<ActionResult<AlbumGetResponse>> GetAlbumsByGenre(Guid genreId)
        {

            var result = await _genreRepository.GetAlbumsByGenre(genreId);
           
            if (!result.Any()) return NotFound();

            return Ok(_mapper.Map<AlbumGetResponse[]>(result));

        }
     
        [HttpPost]
        public async Task<ActionResult<GenreGetResponse>> CreateGenre(GenrePostRequest genre)
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

       
        private string GetResourceUrl(string resourceId)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"{HttpContext.Request.Path}/" + resourceId;
            return locationUrl;
        }

        /// NOTE: PUT will update full entity, which is not good if we have lots of fields.
        /// PATCH is best for partial entity update
        [HttpPut("{genreId}")]
        public async Task<IActionResult> UpdateGenre(Guid genreId, GenrePutRequest genrePutRequest)
        {
            var genreFromRepo = await _genreRepository.GetGenre(genreId);

            // if (genreFromRepo == null)
            // {
            // return NotFound();
            // }

            if (!_genreRepository.GenreExists(genreId))
            { 
                //TODO: Add genre if not exists
                var genreToAdd = _mapper.Map<Genre>(genrePutRequest);
                genreToAdd.Id = genreId;
                _genreRepository.AddGenre(genreToAdd);
                await _genreRepository.SaveAsync();

                var genreGetResponse = _mapper.Map<GenreGetResponse>(genreToAdd);

                return CreatedAtRoute(
                    "GetGenre",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        genreId = genreGetResponse.Id
                    },
                    genreGetResponse);
            }


            _mapper.Map(genrePutRequest,genreFromRepo);

            _genreRepository.UpdateGenre(genreFromRepo);

            await _genreRepository.SaveAsync();

            return NoContent();
        }



        /*
         * opt : replace, remove, add, copy
         * Single PATCH Request Body for replace
         *
            [
                {
	                "opt":"replace",
	                "path":"/name",
	                "value":"updated name"
                }
           ]
         *
         * Single PATCH Request Body for remove
         *
            [
                {
	                "opt":"remove",
	                "path":"/description"	                
                }
           ]
         *
         
         *
         *
         * Multiple PATCH Request Body
         *
            [
                {
	                "opt":"replace",
	                "path":"/name",
	                "value":"updated name"
                },
                {
	                "opt":"replace",
	                "path":"/description",
	                "value":"updated description"
                }
           ]

         *
         * PATCH Request Body for copy
         *
            [
                {
	                "opt":"add",
	                "path":"/description",
                    "value":"New Description"
                }
                ,
                 {
	                "opt":"copy",
	                "from":"/description",
                    "path":"/name"
                }
           ]
         *
         */


        /*
         * ERROR:  "The JSON value could not be converted to Microsoft.AspNetCore.JsonPatch.JsonPatchDocument
         * `1[GsCore.Api.V1.Contracts.Requests.GenreUpdateRequest]. Path: $ | LineNumber: 0 | BytePositionInLine: 1."
         */

        //Package Install: Microsoft.AspNetCore.Mvc.NewtonsoftJson
        [HttpPatch("{genreId}")]
        public async Task<ActionResult> PartialGenreUpdate(Guid genreId,JsonPatchDocument<GenrePatchRequest> patchDocument)
        {
            var genreFromRepo = await _genreRepository.GetGenre(genreId);

            //if (genreFromRepo == null)
            if (!_genreRepository.GenreExists(genreId))
            {
                //TODO : create genre via PATCH request (upserting), if genre is not found in database
                var genreDto=new GenrePatchRequest();
                patchDocument.ApplyTo(genreDto, ModelState);

                if (!TryValidateModel(genreDto))
                {
                    return ValidationProblem(ModelState);
                }

                var genreToAdd= _mapper.Map<Genre>(genreDto);
                genreToAdd.Id = genreId;

                _genreRepository.AddGenre(genreToAdd);
                await _genreRepository.SaveAsync();

                var genreGetResponse = _mapper.Map<GenreGetResponse>(genreToAdd);

                return CreatedAtRoute(
                    "GetGenre",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        genreId = genreGetResponse.Id
                    },
                    genreGetResponse);
            }

            var genreToPatch = _mapper.Map<GenrePatchRequest>(genreFromRepo);
            
            ////TODO: add validation
            patchDocument.ApplyTo(genreToPatch);

            if (!TryValidateModel(genreToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(genreToPatch, genreFromRepo);

            _genreRepository.UpdateGenre(genreFromRepo);

            await _genreRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{genreId}")]
        public ActionResult DeleteGenre(Guid genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
            {
                return NotFound();
            }
            //TODO: Check if any album has this genre id

            return NoContent();
        }



        public override ActionResult ValidationProblem(
           [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
          
        }
    }
}