using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GsCore.Api.V1.Controllers
{
   // [Route("api/v{version:apiVersion}/artists")]
    [Route(ApiRoutes.ArtistsRoute.BaseUrl)]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistController(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _mapper = mapper ?? throw  new ArgumentNullException(nameof(mapper));
        }

        
        [HttpGet]
        public async Task<ActionResult<ArtistGetResponse[]>> GetArtists()
        {
            var artistEntity = await _artistRepository.GetArtistsAsync();
            if (!artistEntity.Any())
            {
                return NotFound();

            }
            return Ok(_mapper.Map<ArtistGetResponse[]>(artistEntity));
        }

      
       [HttpGet(ApiRoutes.ArtistsRoute.Get,Name= "GetArtist")]
        public async Task<ActionResult<ArtistGetResponse>> GetArtists(Guid artistId)
        {

            var artistEntity = await _artistRepository.GetArtistsAsync(artistId);
            if (artistEntity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ArtistGetResponse>(artistEntity));
        }

       
        [HttpPost]
        public async Task<ActionResult<ArtistGetResponse>> CreateArtist([FromBody]ArtistPostRequest artistBase)
        {
          var artistEntity = _mapper.Map<Artist>(artistBase);
           artistEntity.Id=Guid.NewGuid();

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

     
        [HttpPost(ApiRoutes.ArtistsRoute.PostArtistBasicInfo)]
        public async Task<ActionResult<ArtistGetResponse>> CreateBasicInfo(Guid artistId,[FromBody]ArtistBasicInfoPostRequest basicInfo)
        {
           
            if (!_artistRepository.ArtistExists(artistId))
            {
                return NotFound();
            }
            
            if (!_artistRepository.ArtistBasicInfoExists(artistId))
            {

                var artistBasicInfoEntity = _mapper.Map<ArtistBasicInfo>(basicInfo);
                artistBasicInfoEntity.ArtistId = artistId;

                _artistRepository.AddArtistBasicInfo(artistBasicInfoEntity);
                await _artistRepository.SaveAsync();

                return CreatedAtRoute("GetArtist",new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = artistId
                    },null);

            }

            return Conflict(new {message = $"An existing record with the id {artistId} was already found."});
        }


        #region "Update Resourse"
        
        //Update Artist: api/v1/artists/2
        [HttpPut("{artistId}")]
        public async Task<ActionResult> UpdateArtist(Guid artistId,[FromBody]ArtistPutRequest artistPutRequest)
        {
            if (!_artistRepository.ArtistExists(artistId))
            {
                var artistToAdd = _mapper.Map<Artist>(artistPutRequest);
                artistToAdd.Id = artistId;
                _artistRepository.AddArtist(artistToAdd);
                await _artistRepository.SaveAsync();
                var artistGetResponse = _mapper.Map<ArtistGetResponse>(artistToAdd);
                return CreatedAtRoute("GetArtist",new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = artistGetResponse.Id
                    },
                    artistGetResponse);
            }
            var artistFromRepo = await _artistRepository.GetArtistsAsync(artistId);
            _mapper.Map(artistPutRequest, artistFromRepo);
            _artistRepository.UpdateArtist(artistFromRepo);
            await _artistRepository.SaveAsync();
            return NoContent();
        }


        //Update Artist basicInfo :api/v1/artists/2/basicinfo

        [HttpPut(ApiRoutes.ArtistsRoute.PutArtistBasicInfo, Name = "UpdateArtistInfo")]
        public async Task<ActionResult> UpdateArtistInfo(Guid artistId, [FromBody]ArtistInfoPutRequest putRequest)
        {
            if (_artistRepository.ArtistExists(artistId) && !_artistRepository.ArtistBasicInfoExists(artistId))
            {
                var basicInfoToAdd = _mapper.Map<ArtistBasicInfo>(putRequest);
                basicInfoToAdd.ArtistId = artistId;
                _artistRepository.AddArtistBasicInfo(basicInfoToAdd);
                await _artistRepository.SaveAsync();
                var artistGetResponse = _mapper.Map<ArtistGetResponse>(basicInfoToAdd);
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


        #region "Patch"

        [HttpPatch("{artistId}")]
        public async Task<ActionResult> PartialArtistUpdate(Guid artistId,JsonPatchDocument<ArtistPatchRequest> patchRequest)
        {
            if (!_artistRepository.ArtistExists(artistId))
            {
                var artistPatch=new ArtistPatchRequest();
                patchRequest.ApplyTo(artistPatch, ModelState);

                if (!TryValidateModel(artistPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var artistToAdd = _mapper.Map<Artist>(artistPatch);
                artistToAdd.Id = artistId;

                _artistRepository.AddArtist(artistToAdd);

                _artistRepository.SaveAsync();

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

            if (!TryValidateModel(artistToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(artistToPatch, artistFromRepo);
            _artistRepository.UpdateArtist(artistFromRepo);
            await _artistRepository.SaveAsync();
            return NoContent();
        }

        #endregion

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}