using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Api.V1.Helpers;
using GsCore.Api.V1.ResourceParameters;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GsCore.Api.V1.Controllers
{
    // [Route("api/v{version:apiVersion}/artists")]
    /// <summary>
    /// 
    /// </summary>
    [Route(ApiRoutes.ArtistsRoute.BaseUrl)]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;
        private IPropertyMappingService _propertyMappingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistRepository"></param>
        /// <param name="mapper"></param>
        public ArtistController(IArtistRepository artistRepository, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
           
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        #region "GET Request"

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /*
        * GET: /api/v{version}/artists
       */
        [HttpGet(Name ="GetArtists")]
        public async Task<ActionResult<ArtistGetResponse[]>> GetArtists([FromQuery] ArtistResourceParameters artistResourceParameters)
        {

            if (!_propertyMappingService.ValidMappingExistsFor<ArtistGetResponse, Artist>(artistResourceParameters
                .OrderBy))
            {
                return BadRequest();
            }

            var artistFromRepo = await _artistRepository.GetArtistsAsync(artistResourceParameters);
           
            if (!artistFromRepo.Any())
            {
                return NotFound();

            }

            #region "Pagination Metadata"
            
            var previousPageLink = artistFromRepo.HasPrevious ? CreateArtistsResourceUri(artistResourceParameters, ResourceUriType.PreviousPage): null;

            var nextPageLink = artistFromRepo.HasNext ? CreateArtistsResourceUri(artistResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount=artistFromRepo.TotalCount,
                pageSize=artistFromRepo.PageSize, 
                currentPage=artistFromRepo.CurrentPage,
                totalPage=artistFromRepo.TotalPages,
                previousPageLink=previousPageLink,
                nextPageLink=nextPageLink
            };

            /* Add X-Pagination as response header */
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            #endregion

            return Ok(_mapper.Map<ArtistGetResponse[]>(artistFromRepo));
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        /*
         * GET: /api/v{version}/artists/{artistId}
        */
        [HttpGet(ApiRoutes.ArtistsRoute.Get, Name = "GetArtist")]
        public async Task<ActionResult<ArtistGetResponse>> GetArtist(Guid artistId)
        {

            var artistEntity = await _artistRepository.GetArtistsAsync(artistId);
            if (artistEntity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ArtistGetResponse>(artistEntity));
        }

        [HttpGet(ApiRoutes.ArtistsRoute.GetAlbumsByArtist, Name = "GetAlbumByArtist")]
        public async Task<ActionResult<AlbumGetResponse[]>> GetAlbumByArtist(Guid artistId)
        {

            var ablumEntity = await _artistRepository.GetAlbumsByArtistAsync(artistId);
            if (ablumEntity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AlbumGetResponse[]>(ablumEntity));
        }




        #endregion

        #region "POST Request"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistPostRequest"></param>
        /// <returns></returns>


        /*
        * POST: /api/v{version}/artists
        */
        [HttpPost]
        public async Task<ActionResult<ArtistGetResponse>> CreateArtist([FromBody]ArtistPostRequest artistPostRequest)
        {
            var artistEntity = _mapper.Map<Artist>(artistPostRequest);
            artistEntity.Id = Guid.NewGuid();

            _artistRepository.AddArtist(artistEntity);
            await _artistRepository.SaveAsync();
            var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistEntity);
            return CreatedAtRoute(
                "GetArtist",
                new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    artistId = artistGetResponse.Id
                },
                artistGetResponse);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="basicInfo"></param>
        /// <returns></returns>
        /*
        * POST: /api/v{version}/artists/{artistId}/basicInfo
        */
        [HttpPost(ApiRoutes.ArtistsRoute.PostArtistBasicInfo)]
        public async Task<ActionResult<ArtistGetResponse>> CreateBasicInfo(Guid artistId, [FromBody]ArtistBasicInfoPostRequest basicInfo)
        {

            if (!_artistRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            if (_artistRepository.ArtistBasicInfoExists(artistId))
                return Conflict(new { message = $"An existing record with the id {artistId} was already found." });
            var artistBasicInfoEntity = _mapper.Map<ArtistBasicInfo>(basicInfo);
            artistBasicInfoEntity.ArtistId = artistId;

            _artistRepository.AddArtistBasicInfo(artistBasicInfoEntity);
            await _artistRepository.SaveAsync();

            return CreatedAtRoute("GetArtist", new
            {
                version = HttpContext.GetRequestedApiVersion().ToString(),
                artistId = artistId
            }, null);

        }


        #endregion

        #region "PUT Request"

        //Update Artist: api/v1/artists/2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="artistPutRequest"></param>
        /// <returns></returns>
        /*
        * PUT: /api/v{version}/artists/{artistId}
        */
        [HttpPut("{artistId}")]
        public async Task<ActionResult> UpdateArtist(Guid artistId, [FromBody]ArtistPutRequest artistPutRequest)
        {

            var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
            if (!_artistRepository.ArtistExists(artistId))
            {
                var artistToAdd = _mapper.Map<Artist>(artistPutRequest);
                artistToAdd.Id = artistId;
                _artistRepository.AddArtist(artistToAdd);
                await _artistRepository.SaveAsync();
                var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistToAdd);
                return CreatedAtRoute("GetArtist", new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    artistId = artistGetResponse.Id
                },
                    artistGetResponse);
            }

            _mapper.Map(artistPutRequest, artistFromRepo);

            _artistRepository.UpdateArtist(artistFromRepo);

            await _artistRepository.SaveAsync();

            return NoContent();
        }


        //Update Artist basicInfo :api/v1/artists/2/basicinfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="putRequest"></param>
        /// <returns></returns>
        /*
        * PUT: /api/v{version}/artists/{artistId}/basicInfo
        */
        [HttpPut(ApiRoutes.ArtistsRoute.PutArtistBasicInfo, Name = "UpdateArtistInfo")]
        public async Task<ActionResult> UpdateArtistInfo(Guid artistId, [FromBody]ArtistBasicInfoPutRequest putRequest)
        {
            if (_artistRepository.ArtistExists(artistId) && !_artistRepository.ArtistBasicInfoExists(artistId))
            {
                var basicInfoToAdd = _mapper.Map<ArtistBasicInfo>(putRequest);
                basicInfoToAdd.ArtistId = artistId;
                _artistRepository.AddArtistBasicInfo(basicInfoToAdd);
                await _artistRepository.SaveAsync();

                var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
                var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistFromRepo);

                return CreatedAtRoute("GetArtist", new
                {
                    version = HttpContext.GetRequestedApiVersion().ToString(),
                    artistId = artistGetResponse.Id
                }, artistGetResponse);

            }
            var basicInfoFromRepo = await _artistRepository.GetArtistBasicInfo(artistId);
            _mapper.Map(putRequest, basicInfoFromRepo);
            _artistRepository.UpdateArtistBasicInfo(basicInfoFromRepo);
            await _artistRepository.SaveAsync();
            return NoContent();
        }


        #endregion

        #region "PATCH Request"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="patchRequest"></param>
        /// <returns></returns>
        /*
         * PATCH: /api/v{version}/artists/{artistId}
        */
        [HttpPatch("{artistId}")]
        public async Task<ActionResult> PartialArtistUpdate(Guid artistId, JsonPatchDocument<ArtistPatchRequest> patchRequest)
        {
            if (!_artistRepository.ArtistExists(artistId))
            {
                var artistPatch = new ArtistPatchRequest();
                patchRequest.ApplyTo(artistPatch, ModelState);

                //TODO: Validation 
                if (!TryValidateModel(artistPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var artistToAdd = _mapper.Map<Artist>(artistPatch);
                artistToAdd.Id = artistId;

                _artistRepository.AddArtist(artistToAdd);

                await _artistRepository.SaveAsync();

                var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistToAdd);
                return CreatedAtRoute(
                    "GetArtist",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = artistGetResponse.Id
                    },
                    artistGetResponse);
            }

            var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
            var artistToPatch = _mapper.Map<ArtistPatchRequest>(artistFromRepo);

            patchRequest.ApplyTo(artistToPatch);
            //TODO: Validation
            if (!TryValidateModel(artistToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(artistToPatch, artistFromRepo);
            artistFromRepo.DateModified=DateTime.UtcNow;
            _artistRepository.UpdateArtist(artistFromRepo);
            await _artistRepository.SaveAsync();
            return NoContent();
        }

        /*
         * PATCH: /api/v{version}/artists/{artistId}/basicInfo
        */
        [HttpPatch(ApiRoutes.ArtistsRoute.PatchArtistBasicInfo, Name= "UpdateArtistInfo")]
        public async Task<ActionResult> PartialArtistBasicInfoUpdate(Guid artistId, JsonPatchDocument<ArtistBasicInfoPatchRequest> patchRequest)
        {
            if (_artistRepository.ArtistExists(artistId) && !_artistRepository.ArtistBasicInfoExists(artistId))
            {
                var artistInfoPatch = new ArtistBasicInfoPatchRequest();
                patchRequest.ApplyTo(artistInfoPatch, ModelState);

                //TODO: Validation 
                if (!TryValidateModel(artistInfoPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var artistToAdd = _mapper.Map<ArtistBasicInfo>(artistInfoPatch);
                artistToAdd.ArtistId = artistId;

                _artistRepository.AddArtistBasicInfo(artistToAdd);
                await _artistRepository.SaveAsync();

                var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
                var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistFromRepo);
                return CreatedAtRoute(
                    "GetArtist",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = artistGetResponse.Id
                    },
                    artistGetResponse);
            }

            var artistInfoFromRepo = await _artistRepository.GetArtistBasicInfo(artistId);
            var artistInfoToPatch = _mapper.Map<ArtistBasicInfoPatchRequest>(artistInfoFromRepo);


            patchRequest.ApplyTo(artistInfoToPatch);
            //TODO: Validation
            if (!TryValidateModel(artistInfoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(artistInfoToPatch, artistInfoFromRepo);
            artistInfoFromRepo.DateModified=DateTime.UtcNow;
            _artistRepository.UpdateArtistBasicInfo(artistInfoFromRepo);
            await _artistRepository.SaveAsync();
            return NoContent();
        }
        #endregion

        #region "DELETE Request"

        /* DO NOT expose delete endpoint */
        ////[HttpDelete("{artistId}")]
        ////public async Task<ActionResult> Delete(Guid artistId)
        ////{
        ////    var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
        ////    if (artistFromRepo == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    _artistRepository.Delete(artistFromRepo);
        ////    await _artistRepository.SaveAsync();

        ////    return NoContent();
        ////}

        #endregion


        #region "PATCH Validation"
        
        public override ActionResult ValidationProblem(
            [ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        #endregion

        #region Create Resource Uri"
        
        private string CreateArtistsResourceUri(ArtistResourceParameters artistResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetArtists", new
                    {
                        pageNumber = artistResourceParameters.PageNumber - 1,
                        pageSize = artistResourceParameters.PageSize,
                        searchQuery = artistResourceParameters.SearchQuery,
                        /* Adding OrderBy Clause to pagination links */
                        orderBy=artistResourceParameters.OrderBy
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetArtists", new
                    {
                        pageNumber = artistResourceParameters.PageNumber + 1,
                        pageSize = artistResourceParameters.PageSize,
                        searchQuery = artistResourceParameters.SearchQuery,
                        /* Adding OrderBy Clause to pagination links */
                        orderBy = artistResourceParameters.OrderBy
                    });
                default:
                    return Url.Link("GetArtists", new
                    {
                        pageNumber = artistResourceParameters.PageNumber,
                        pageSize = artistResourceParameters.PageSize,
                        searchQuery = artistResourceParameters.SearchQuery,
                        /* Adding OrderBy Clause to pagination links */
                        orderBy = artistResourceParameters.OrderBy
                    });
            }
        }

        #endregion

    }
}