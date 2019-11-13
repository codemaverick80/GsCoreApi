namespace GsCore.Api.V1
{
    public static class ApiRoutes
    {
        const string Root = "api";
        const string Version = "v1";
        const string Base = Root + "/" + Version; //  api/v1

        public static class GenresRoute
        {
            /// <summary>
            /// GET: api/v1/genres
            /// </summary>
            public const string GetAll = Base + "/genres";

            /// <summary>
            /// PUT: api/v1/genres/12
            /// </summary>
            public const string Update = Base + "/genres/{genreId}";

            /// <summary>
            /// DELETE: api/v1/genres/12
            /// </summary>
            public const string Delete = Base + "/genres/{genreId}";

            /// <summary>
            /// GET: api/v1/genres/12
            /// </summary>
            public const string Get = Base + "/genres/{genreId}";

            /// <summary>
            /// POST: api/v1/genres
            /// </summary>
            public const string Create = Base + "/genres";
        }
    }
}
