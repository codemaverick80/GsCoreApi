using AutoMapper;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.MappingProfiles
{
    public class TrackMappingProfile: Profile
    {
        public TrackMappingProfile()
        {
            CreateMap<Track, TrackGetResponse>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.TrackName));
            CreateMap<TrackGetResponse, Track>()
                .ForMember(dest => dest.TrackName, source => source.MapFrom(src => src.Name));

            CreateMap<TrackPostRequest, Track>()
                .ForMember(dest => dest.TrackName, source => source.MapFrom(src => src.Name));

            CreateMap<TrackPutRequest,Track>()
                .ForMember(dest => dest.TrackName, source => source.MapFrom(src => src.Name));

            CreateMap<TrackPatchRequest, Track>()
                .ForMember(dest => dest.TrackName, source => source.MapFrom(src => src.Name));
            CreateMap<Track, TrackPatchRequest>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.TrackName));

        }
    }
}
