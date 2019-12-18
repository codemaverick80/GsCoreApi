using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace GsCore.Api.V1
{
    public static class ApiRoutes
    {
        const string Root = "api";
        const string Version = "v{version:apiVersion}";
        const string Base = Root + "/" + Version;

        public static class RootRoute
        {
            public const string Root = Base;
        }

        public static class GenresRoute
        {
            public const string BaseUrl = Base + "/genres";
            public const string Get = "{genreId}";
            public const string GetAlbumByGenre = "{genreId}/albums";
        }

        public static class ArtistsRoute
        {
            public const string BaseUrl = Base + "/artists";
            public const string Get = "{artistId}";
            public const string GetAlbumsByArtist = "{artistId}/albums";
            public const string PostArtistBasicInfo = "{artistId}/basicInfo";
            public const string PutArtistBasicInfo = "{artistId}/basicInfo";
            public const string PatchArtistBasicInfo = "{artistId}/basicInfo";
        }

        public static class AlbumsRoute
        {
            public const string BaseUrl = Base + "/albums";
            public const string Get ="{albumId}";
            public const string GetTrackByAlbum = "{albumId}/tracks";
            
            public const string PostTrack = "{albumId}/tracks";

            public const string PutAlbum = "{albumId}";
            public const string PutTrack = "{albumId}/tracks/{trackId}";

            public const string PatchAlbum = "{albumId}";
            public const string PatchTrack = "{albumId}/tracks/{trackId}";
        }
    }
}
