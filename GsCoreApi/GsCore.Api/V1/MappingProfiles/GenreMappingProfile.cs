using AutoMapper;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.MappingProfiles
{
    public class GenreMappingProfile: Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<Genre, GenreGetResponse>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.GenreName));
            CreateMap<GenreGetResponse, Genre>()
                .ForMember(dest => dest.GenreName, source => source.MapFrom(src => src.Name));

            CreateMap<GenrePostRequest, Genre>()
                .ForMember(dest => dest.GenreName, source => source.MapFrom(src => src.Name));

            CreateMap<GenrePutRequest, Genre>()
                .ForMember(dest => dest.GenreName, source => source.MapFrom(src => src.Name));

            CreateMap<GenrePatchRequest, Genre>()
                .ForMember(dest => dest.GenreName, source => source.MapFrom(src => src.Name));
            CreateMap<Genre, GenrePatchRequest>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.GenreName));

        }
    }
}
