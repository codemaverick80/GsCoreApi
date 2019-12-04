using AutoMapper;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.MappingProfiles
{
    public class AlbumMappingProfile:Profile
    {
        public AlbumMappingProfile()
        {
            CreateMap<Album, AlbumGetResponse>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.AlbumName));

            CreateMap<AlbumGetResponse, Album>()
                .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));

            CreateMap<Album, AlbumCreateRequest>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.AlbumName));

            CreateMap<AlbumCreateRequest, Album>()
                .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));

        }
    }
}
