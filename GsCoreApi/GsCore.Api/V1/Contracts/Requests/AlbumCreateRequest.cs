using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class AlbumCreateRequest
    {
       [Required(ErrorMessage = "Album name should not be empty.")]
       [MaxLength(200)]
        public string Name { get; set; }
       
        [Required(ErrorMessage = "Album must have a artist id.")]
        public Guid ArtistId { get; set; }
        
        [Required(ErrorMessage = "Album must have a genre id.")]
        public Guid GenreId { get; set; }
        public int? Rating { get; set; }
        public int? Year { get; set; }

        [MaxLength(200)]
        public string Label { get; set; }

        [MaxLength(50)]
        public string ThumbnailTag { get; set; }

        [MaxLength(100)]
        public string SmallThumbnail { get; set; }
        [MaxLength(100)]
        public string MediumThumbnail { get; set; }
        [MaxLength(100)]
        public string LargeThumbnail { get; set; }

        [MaxLength(500)]
        public string AlbumUrl { get; set; }

        public ICollection<TrackCreateRequest> Tracks { get; set; }=new List<TrackCreateRequest>();
    
    }
}
