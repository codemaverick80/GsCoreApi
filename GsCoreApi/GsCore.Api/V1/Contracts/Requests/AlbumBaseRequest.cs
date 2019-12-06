using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Requests.Post;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace GsCore.Api.V1.Contracts.Requests
{
    public abstract class AlbumBaseRequest
    {
        [Required(ErrorMessage = "Album name should not be empty.")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Album must have a artist id.")]
        public Guid ArtistId { get; set; }

        [Required(ErrorMessage = "Album must have a genre id.")]
        public Guid GenreId { get; set; }
       
        public virtual int? Rating { get; set; }
       
        public virtual int? Year { get; set; }

        [MaxLength(200)]
        public virtual string Label { get; set; }

        [MaxLength(50)]
        public virtual string ThumbnailTag { get; set; }

        [MaxLength(100)]
        public virtual string SmallThumbnail { get; set; }
       
        [MaxLength(100)]
        public  virtual string MediumThumbnail { get; set; }
        
        [MaxLength(100)]
        public virtual string LargeThumbnail { get; set; }

        [MaxLength(500)]
        public virtual string AlbumUrl { get; set; }

        public virtual ICollection<TrackPostRequest> Tracks { get; set; }=new List<TrackPostRequest>();

    }
}
