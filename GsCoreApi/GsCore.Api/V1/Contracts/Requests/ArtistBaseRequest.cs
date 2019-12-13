using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Requests.Post;
using GsCore.Api.V1.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GsCore.Api.V1.Contracts.Requests
{
    [ArtistMustHaveUniqueFirstNameAndLastName]
    public abstract class ArtistBaseRequest
    {
      
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [MaxLength(50)]
        public virtual string YearActive { get; set; }

        public virtual string Biography { get; set; }
      
        [MaxLength(50)]
        public virtual string ThumbnailTag { get; set; }
       
        [MaxLength(100)]
        public virtual string SmallThumbnail { get; set; }
        
        [MaxLength(100)]
        public virtual string LargeThumbnail { get; set; }

        public bool IsDeleted { get; set; }

        public ArtistBasicInfoPostRequest BasicInfo { get; set; }
    }
}
