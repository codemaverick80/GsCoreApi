using System;

namespace GsCore.Api.V2.Contracts.Responses
{
    public class GenreGetResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
