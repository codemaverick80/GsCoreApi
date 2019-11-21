using System;
using System.Collections.Generic;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class AlbumCreateRequest
    {
       
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

        public ICollection<TrackCreateRequest> Tracks { get; set; }=new List<TrackCreateRequest>();
    
    }
}
