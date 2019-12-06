using AutoMapper;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
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

            CreateMap<AlbumPostRequest, Album>()
                .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));

            CreateMap<AlbumPutRequest, Album>()
                .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));

            CreateMap<AlbumPatchRequest, Album>()
                .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));
            CreateMap<Album, AlbumPatchRequest>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.AlbumName));

        }
    }
}
