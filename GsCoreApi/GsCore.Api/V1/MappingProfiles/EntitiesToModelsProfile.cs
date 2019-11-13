using AutoMapper;
using GsCore.Api.V1.Contracts.Requests;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.MappingProfiles
{
    public class EntitiesToModelsProfile: Profile
    {
        public EntitiesToModelsProfile()
        {


            //        CreateMap<Artists, ArtistGetResponse>()
            //                .ForMember(dest=> dest.Born, source=>source.MapFrom(src=> src.ArtistBasicInfo.Born))
            //                .ForMember(dest => dest.AlsoKnownAs, source => source.MapFrom(src => src.ArtistBasicInfo.Aka))
            //                .ForMember(dest => dest.Died, source => source.MapFrom(src => src.ArtistBasicInfo.Died))
            //                //.ForMember(dest => dest.Albums, source => source.MapFrom(src => src.Albums.Select(x =>
            //                //        new AlbumModel
            //                //        {
            //                //            Album = x.Album,
            //                //            Year = x.Year,
            //                //            Label = x.Label,
            //                //            AlbumUrl = x.AlbumUrl,
            //                //            ThumbnailL = x.ThumbnailL,
            //                //            ThumbnailM = x.ThumbnailM,
            //                //            ThumbnailS = x.ThumbnailS,
            //                //            Rating = x.Rating,
            //                //            UserRating = x.UserRating
            //                //        })))
            //                ;


            ////            CreateMap<Albums, AlbumsModel>()
            ////                .ForMember(dest=>dest.ReleaseYear,source=>source.MapFrom(src=>src.Year));

            CreateMap<Album, AlbumGetResponse>()
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.AlbumName));


            //CreateMap<AlbumGetResponse,Album>()
            //    .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));


            //CreateMap<AlbumGetResponse,Album>()
            //    .ForMember(dest => dest.AlbumName, source => source.MapFrom(src => src.Name));


            //CreateMap<Tracks, TrackGetResponse>()
            //    .ForMember(dest => dest.TrackId, source => source.MapFrom(src => src.Id));

            CreateMap<Genre, GenreGetResponse>();

          
            
            
            CreateMap<Genre, GenreCreateRequest>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.GenreName))
                .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Description));

            CreateMap<GenreCreateRequest, Genre>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.GenreName, source => source.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Description));

        }

    }
}
