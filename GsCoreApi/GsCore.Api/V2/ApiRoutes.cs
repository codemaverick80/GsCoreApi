namespace GsCore.Api.V2
{
    public static class ApiRoutes
    {
        const string Root = "api";
        const string Version = "v{version:apiVersion}";
        const string Base = Root + "/" + Version; //  api/v1

        public static class GenresRoute
        {
            /// <summary>
            /// GET: api/v2/genres
            /// </summary>
            public const string GetAll = Base + "/genres";

            /// <summary>
            /// PUT: api/v2/genres/12
            /// </summary>
            public const string Update = Base + "/genres/{genreId}";

            /// <summary>
            /// DELETE: api/v2/genres/12
            /// </summary>
            public const string Delete = Base + "/genres/{genreId}";

            /// <summary>
            /// GET: api/v2/genres/12
            /// </summary>
            public const string Get = Base + "/genres/{genreId}";

            /// <summary>
            /// POST: api/v2/genres
            /// </summary>
            public const string Create = Base + "/genres";
        }
    }
}
