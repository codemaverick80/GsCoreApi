using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GsCore.Api.Services.Repository.Interfaces;
using GsCore.Api.V1.Contracts.Responses;
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
            return Ok(_mapper.Map<ArtistGetResponse[]>(artistEntity));
        }

        [HttpGet("{artistId}")]
        public async Task<ActionResult<ArtistGetResponse>> GetArtists(int artistId)
        {

            var artistEntity = await _artistRepository.GetArtistsAsync(artistId);
            return Ok(_mapper.Map<ArtistGetResponse>(artistEntity));
        }




    }
}