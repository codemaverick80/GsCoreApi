using System;

namespace GsCore.Api.V2.Contracts.Requests
{
    public class GenreCreateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
