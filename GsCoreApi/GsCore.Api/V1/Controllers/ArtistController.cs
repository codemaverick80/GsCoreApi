using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GsCore.Api.V1.Controllers
{
    [Route("api/v{version:apiVersion}/artists")]
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

        [HttpGet("{artistId}",Name="GetArtist")]
        public async Task<ActionResult<ArtistGetResponse>> GetArtists(int artistId)
        {

            var artistEntity = await _artistRepository.GetArtistsAsync(artistId);
            if (artistEntity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ArtistGetResponse>(artistEntity));
        }


        [HttpPost]
        public async Task<ActionResult<ArtistGetResponse>> CreateArtist([FromBody]ArtistCreateRequest artist)
        {
            var artistEntity = _mapper.Map<Artist>(artist);
           
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

        //POST: https://localhost:44336/api/v1/artists/778/basicInfo
        [HttpPost("{artistId}/basicInfo")]
        public async Task<ActionResult<ArtistGetResponse>> CreateBasicInfo(int artistId,[FromBody]ArtistBasicInfoCreateRequest basicInfo)
        {
            // Check if artist exists
            if (!_artistRepository.ArtistExists(artistId))
            {
                return NotFound();
            }
            //check if basic info exists, if not create it
            if (!_artistRepository.ArtistBasicInfoExists(artistId))
            {

                var artistBasicInfoEntity = _mapper.Map<ArtistBasicInfo>(basicInfo);
                artistBasicInfoEntity.ArtistId = artistId;

                _artistRepository.AddArtistBasicInfo(artistBasicInfoEntity);
                await _artistRepository.SaveAsync();

                return CreatedAtRoute(
                    "GetArtist",
                    new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        artistId = artistId
                    },null);

            }

            return Conflict(new {message = $"An existing record with the id {artistId} was already found."});
        }






    }
}