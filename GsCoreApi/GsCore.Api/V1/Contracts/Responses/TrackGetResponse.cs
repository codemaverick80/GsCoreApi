using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Responses
{
    public class TrackGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AlbumId { get; set; }
        public string Composer { get; set; }
        public string Performer { get; set; }
        public string Featuring { get; set; }
        public string Duration { get; set; }
    }
}
