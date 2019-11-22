using System;

namespace GsCore.Api.V1.Contracts.Responses
{
    public class GenreGetResponse
    {
        /// <summary>
        /// Genre id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Genre Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Genre Description
        /// </summary>
        public string Description { get; set; }
    }
}
