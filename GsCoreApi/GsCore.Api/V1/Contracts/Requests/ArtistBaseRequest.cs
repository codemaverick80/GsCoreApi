using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GsCore.Api.V1.Contracts.Requests
{
    public abstract class ArtistBaseRequest
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public virtual string YearActive { get; set; }

        public virtual string Biography { get; set; }
      
        [MaxLength(50)]
        public virtual string ThumbnailTag { get; set; }
       
        [MaxLength(100)]
        public virtual string SmallThumbnail { get; set; }
        
        [MaxLength(100)]
        public virtual string LargeThumbnail { get; set; }

        public ArtistBasicInfoCreateRequest BasicInfo { get; set; }
    }
}
