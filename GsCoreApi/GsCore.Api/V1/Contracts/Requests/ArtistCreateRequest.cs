using System;
using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class ArtistCreateRequest
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string YearActive { get; set; }

        public string Biography { get; set; }
        [MaxLength(50)]
        public string ThumbnailTag { get; set; }
        [MaxLength(100)]
        public string SmallThumbnail { get; set; }
        [MaxLength(100)]
        public string LargeThumbnail { get; set; }

        public ArtistBasicInfoCreateRequest BasicInfo { get; set; }
        
    }
}
