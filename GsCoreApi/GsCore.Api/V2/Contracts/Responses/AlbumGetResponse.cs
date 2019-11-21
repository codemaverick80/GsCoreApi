using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V2.Contracts.Responses
{
    public class AlbumGetResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ArtistId { get; set; }
        public Guid GenreId { get; set; }
        public int? Rating { get; set; }
        public int? Year { get; set; }
        public string Label { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string MediumThumbnail { get; set; }
        public string LargeThumbnail { get; set; }
        public string AlbumUrl { get; set; }
    }
}
