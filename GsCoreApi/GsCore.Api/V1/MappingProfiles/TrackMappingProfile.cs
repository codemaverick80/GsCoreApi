using AutoMapper;
using GsCore.Api.V1.Contracts.Requests;
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


            CreateMap<Track, TrackCreateRequest>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.TrackName));

            CreateMap<TrackCreateRequest, Track>()
                .ForMember(dest => dest.TrackName, source => source.MapFrom(src => src.Name));

        }
    }
}
