using AutoMapper;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Requests.Patch;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.Contracts.Requests.Put;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.MappingProfiles
{
    public class ArtistMappingProfile : Profile
    {
        public ArtistMappingProfile()
        {

            CreateMap<Artist, ArtistGetResponse>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.BasicInfo, source => source.MapFrom(src => src.ArtistBasicInfo));
           
            //CreateMap<ArtistGetResponse, Artist>()
            //    .ForMember(dest => $"{dest.FirstName} {dest.LastName}", source => source.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.ArtistBasicInfo, source => source.MapFrom(src => src.BasicInfo));

            CreateMap<ArtistPostRequest, Artist>()
                  //.ForMember(dest => dest.ArtistName, source => source.MapFrom(src => src.Name))
                  .ForMember(dest => dest.ArtistBasicInfo, source => source.MapFrom(src => src.BasicInfo));

            CreateMap<ArtistPutRequest, Artist>()
                       //.ForMember(dest => dest.ArtistName, source => source.MapFrom(src => src.Name))
                       .ForMember(dest => dest.ArtistBasicInfo, source => source.MapFrom(src => src.BasicInfo));

            CreateMap<ArtistPatchRequest, Artist>()
                //.ForMember(dest => dest.ArtistName, source => source.MapFrom(src => src.Name))
                .ForMember(dest => dest.ArtistBasicInfo, source => source.MapFrom(src => src.BasicInfo));
           
            //CreateMap<Artist, ArtistPatchRequest>()
            //    //.ForMember(dest => dest.Name, source => source.MapFrom(src => src.ArtistName))
            //    .ForMember(dest => dest.BasicInfo, source => source.MapFrom(src => src.ArtistBasicInfo));

            

            CreateMap<ArtistBasicInfo, ArtistBasicInfoGetResponse>();
           // CreateMap<ArtistBasicInfoGetResponse, ArtistBasicInfo>();

            CreateMap<ArtistBasicInfoPostRequest, ArtistBasicInfo>();

            CreateMap<ArtistBasicInfoPutRequest, ArtistBasicInfo>();

            CreateMap<ArtistBasicInfoPatchRequest, ArtistBasicInfo>();
          

        }

    }
}
